import React from 'react';
import ReactDOM from 'react-dom/client';

import 'font-awesome/css/font-awesome.css';
import './styles/style.scss';
import 'react-country-state-city/dist/react-country-state-city.css';
import App from './components/app/App';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);
