import axios from "axios";
import authStore from "stores/authStore";

export const api = axios.create({
	baseURL: "https://localhost:7195"
});

// создаем перехватчик запросов
// который к каждому запросу добавляет accessToken из localStorage
api.interceptors.request.use(config => {
	console.log("Config use accessToken", config);
	if (
		localStorage.getItem("accessToken") === null ||
		localStorage.getItem("accessToken") === "" ||
		localStorage.getItem("accessToken") === undefined
	) {
		return config;
	}

	// if (config.url === "/refresh") {
	// 	authStore.logout();
	// 	return config;
	// }
	config.headers.Authorization = `Bearer ${localStorage.getItem("accessToken")}`;
	return config;
});

// создаем перехватчик ответов
// который в случае невалидного accessToken попытается его обновить
// и переотправить запрос с обновленным accessToken
api.interceptors.response.use(
	// в случае валидного accessToken ничего не делаем:
	config => {
		return config;
	},
	// в случае просроченного accessToken пытаемся его обновить:
	async error => {
		// предотвращаем зацикленный запрос, добавляя свойство _isRetry
		const originalRequest = { ...error.config };
		originalRequest._isRetry = true;
		console.log("Intersept!!!");

		if (
			// проверим, что ошибка именно из-за невалидного accessToken
			(error.code === "ERR_NETWORK" || error.response.status === 401) &&
			// проверим, что запрос не повторный
			error.config &&
			!error.config._isRetry
		) {
			try {
				if (
					localStorage.getItem("refreshToken") === null ||
					localStorage.getItem("refreshToken") === "" ||
					localStorage.getItem("refreshToken") === undefined
				) {
					console.log("RefreshToken is null");
					return;
				}
				console.log("Start refreshing token");
				// запрос на обновление токенов
				const resp = await api.post("/refresh", { refreshToken: localStorage.getItem("refreshToken") });

				// сохраняем новый accessToken в localStorage
				localStorage.setItem("accessToken", resp.data.accessToken);
				// TODO Исправить костыль | Нужно при истечение refreshToken перебрасывать пользователя на login
				localStorage.setItem("refreshToken", resp.data.refreshToken);
				// переотправляем запрос с обновленным accessToken
				return api.request(originalRequest);
			} catch (error) {
				console.log("AUTH ERROR");
				window.location.href = "/login";
			}
		}
		// на случай, если возникла другая ошибка (не связанная с авторизацией)
		// пробросим эту ошибку
		throw error;
	}
);
