import React, { useState } from 'react';
import Pagination from '../appPagination/Pagination';
import './weatherChangesTable.scss';

const WeatherChangesTable = ({ weatherChanges, selectedParameter }) => {
    const [currentPage, setCurrentPage] = useState(1);
    const changesPerPage = 8;

    const indexOfLastChange = currentPage * changesPerPage;
    const indexOfFirstChange = indexOfLastChange - changesPerPage;
    const currentChanges = weatherChanges.slice(indexOfFirstChange, indexOfLastChange);

    const paginate = (pageNumber) => setCurrentPage(pageNumber);

    const formatDate = (dateString) => {
        if (!dateString) return '';
        return dateString.replace('T', ' ').replace('Z', '');
    };

    const gradientColors = {
        extremeCold: 'rgba(0, 0, 255, 0.2)', 
        veryCold: 'rgba(173, 216, 230, 0.5)',
        cold: 'rgba(135, 206, 250, 0.7)',    
        cool: 'rgba(173, 216, 230, 0.5)',    
        mild: 'rgba(240, 248, 255, 1)',      
        warm: 'rgba(255, 255, 224, 0.7)',    
        hot: 'rgba(255, 165, 0, 0.5)',       
        veryHot: 'rgba(255, 69, 0, 0.5)',    
        extremeHot: 'rgba(255, 0, 0, 0.2)',  
    };

    const getGradient = (temperature) => {
        let startColor, endColor;
        if (temperature <= -30) {
            startColor = gradientColors.extremeCold;
            endColor = gradientColors.veryCold;
        } else if (temperature > -30 && temperature <= -10) {
            startColor = gradientColors.veryCold;
            endColor = gradientColors.cold;
        } else if (temperature > -10 && temperature <= 0) {
            startColor = gradientColors.cold;
            endColor = gradientColors.cool;
        } else if (temperature > 0 && temperature <= 10) {
            startColor = gradientColors.cool;
            endColor = gradientColors.mild;
        } else if (temperature > 10 && temperature <= 20) {
            startColor = gradientColors.mild;
            endColor = gradientColors.warm;
        } else if (temperature > 20 && temperature <= 30) {
            startColor = gradientColors.warm;
            endColor = gradientColors.hot;
        } else if (temperature > 30 && temperature <= 40) {
            startColor = gradientColors.hot;
            endColor = gradientColors.veryHot;
        } else if (temperature > 40 && temperature <= 50) {
            startColor = gradientColors.veryHot;
            endColor = gradientColors.extremeHot;
        } else {
            startColor = gradientColors.extremeHot;
            endColor = gradientColors.extremeHot;
        }
        return `linear-gradient(to right, ${startColor}, ${endColor})`;
    };

    return (
        <>
            {weatherChanges.length === 0 ? (
                    <p className="no-weather">
                        Weather not found
                    </p>
                ) : (
                    <div className="weather-details-modal-weather-changes-table">
                        <ul className="weather-details-modal-changes-list">
                            {currentChanges.map((change, index) => (
                                <li key={index} className="weather-details-modal-change-item" style={{ background: getGradient(change.temperatureCelsius) }}>
                                    {selectedParameter === 'all' && (
                                        <div>
                                            <p className='weather weather__date'><i className="fa fa-clock-o"></i> {formatDate(change.date)}</p>
                                            <span className='weather weather__temperatureCelsius'>Temperature: {change.temperatureCelsius}°C</span>
                                            <span className='weather weather__humidity'>Humidity: {change.humidity}%</span>
                                            <span className='weather weather__pressureIn'>Pressure: {change.pressureIn} mm Hg</span>
                                            <span className='weather weather__windSpeedKph'>Wind speed: {change.windSpeedKph} km/h</span>
                                        </div>
                                    )}
                                    {selectedParameter === 'temperature' && (
                                        <div>
                                            <p className='weather weather__date'><i className="fa fa-clock-o"></i> {formatDate(change.date)}</p>
                                            <span className='weather weather__temperatureCelsius'>Temperature: {change.temperatureCelsius}°C</span>
                                        </div>
                                    )}
                                    {selectedParameter === 'humidity' && (
                                        <div>
                                            <p className='weather weather__date'><i className="fa fa-clock-o"></i> {formatDate(change.date)}</p>
                                            <span className='weather weather__humidity'>Humidity: {change.humidity}%</span>
                                        </div>
                                    )}
                                    {selectedParameter === 'pressure' && (
                                        <div>
                                            <p className='weather weather__date'><i className="fa fa-clock-o"></i> {formatDate(change.date)}</p>
                                            <span className='weather weather__pressureIn'>Pressure: {change.pressureIn} mm Hg</span>
                                        </div>
                                    )}
                                    {selectedParameter === 'windSpeed' && (
                                        <div>
                                            <p className='weather weather__date'><i className="fa fa-clock-o"></i> {formatDate(change.date)}</p>
                                            <span className='weather weather__windSpeedKph'>Wind speed: {change.windSpeedKph} km/h</span>
                                        </div>
                                    )}
                                </li>
                            ))}
                        </ul>
                        {weatherChanges.length > changesPerPage && (
                            <Pagination
                                totalItems={weatherChanges.length}
                                itemsPerPage={changesPerPage}
                                currentPage={currentPage}
                                paginate={paginate}
                            />
                        )}
                    </div>
                )
            }
        </>
    );
};

export default WeatherChangesTable;
