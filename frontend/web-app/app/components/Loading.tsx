import React from 'react';

// Define the component
export default function LoadingComponent() {
    return (
        <div style={{
            fontSize: '3em',
            textAlign: 'center',
            color: '#0A6847',
            fontWeight: 'bold',
            textShadow: '2px 2px 8px rgba(0, 0, 0, 0.5)',
            animation: 'breathe 3s ease-in-out infinite'
        }}>
            Loading...
            <style>
                {`
          @keyframes breathe {
            0%, 100% {
              transform: scale(1);
              opacity: 1;
              text-shadow: 2px 2px 8px rgba(0, 0, 0, 0.5);
            }
            50% {
              transform: scale(1.5);
              opacity: 0.5;
              text-shadow: 2px 2px 12px rgba(0, 0, 0, 0.8);
            }
          }
        `}
            </style>
        </div>
    );
}
