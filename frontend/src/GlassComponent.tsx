import React, { useEffect, useState } from 'react';
import { motion, useAnimation } from 'framer-motion';
import { Outlet } from 'react-router-dom';
// import './GlassComponent.scss'; // import your SCSS file

const GlassComponent: React.FC = () => {
    const [circles, setCircles] = useState<any[]>([]);
  
    useEffect(() => {
      const createCircles = () => {
        const newCircles = [];
        for (let i = 0; i < 50; i++) {
          newCircles.push({
            id: i,
            x: Math.random() * window.innerWidth,
            y: Math.random() * window.innerHeight,
            size: Math.random() * 20 + 5,
            speedX: Math.random() * 4 - 2,
            speedY: Math.random() * 4 - 2,
          });
        }
        setCircles(newCircles);
      };
  
      createCircles();
  
      const interval = setInterval(createCircles, 5000000);
  
      return () => clearInterval(interval);
    }, []);
  
    return (
      <div className="relative w-full h-screen overflow-hidden">
        {/* Background animation */}
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          exit={{ opacity: 0 }}
          transition={{ duration: 0.5 }}
          className="absolute inset-0 bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500"
        />
  
        {/* Glass effect */}
        <div className="absolute inset-0 backdrop-filter backdrop-blur-sm bg-opacity-30"></div>
  
        {/* Animated mini circles */}
        {circles.map((circle: { id: React.Key | null | undefined; size: any; y: number; x: number; speedX: any; speedY: any; }) => (
          <motion.div
            key={circle.id}
            style={{
              width: circle.size,
              height: circle.size,
              borderRadius: '50%',
              backgroundColor: 'rgba(255, 255, 255, 0.3)',
              position: 'absolute',
              top: circle.y,
              left: circle.x,
            }}
            animate={{
                x: circle.x + circle.speedX,
                y: circle.y + circle.speedY,
            }}
            transition={{
              duration: circle.speedX * 200,
              repeat: Infinity,
              repeatType: 'reverse',
              ease: 'linear',
            }}
          />
        ))}
  
        {/* Content */}
        <div className="relativ h-full flex flex-col">
          <Outlet />
        </div>
      </div>
    );
  };

export default GlassComponent;