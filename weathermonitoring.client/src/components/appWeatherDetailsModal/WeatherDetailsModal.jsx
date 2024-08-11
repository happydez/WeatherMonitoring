import React, { useState, useEffect } from 'react';
import Loader from '../appLoader/Loader';
import WeatherChangesTable from '../appWeatherChangesTable/WeatherChangesTable';
import CurrentTime from '../appCurrentTime/CurrentTime';
import { Line } from 'react-chartjs-2';
import 'chart.js/auto';
import './weatherDetailsModal.scss';

import moment from 'moment-timezone';

const WeatherDetailsModal = ({ city, onClose, weatherService }) => {
    const [weatherData, setWeatherData] = useState(null);
    const [view, setView] = useState('current'); // 'current' or 'changes'
    const [selectedParameter, setSelectedParameter] = useState('all');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [weatherChanges, setWeatherChanges] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [errorMessage, setErrorMessage] = useState('');

    useEffect(() => {
        const fetchWeatherData = async () => {
            try {
                const data = await weatherService.getWeathers(city.id, 1, 0, true);
                setWeatherData(data[0]);

                const changes = await weatherService.getWeathers(city.id, 24, 0, true);
                setWeatherChanges(changes);
            } catch (error) {
                console.error("Failed to fetch weather data:", error);
            } finally {
                setIsLoading(false);
            }
        };

        fetchWeatherData();
    }, [city.id, weatherService]);

    const getUtcOffsetForTimeZone = (timeZoneId) => {
        const now = moment.tz(timeZoneId);
        const offset = now.utcOffset() / 60;
        return `${offset < 0 ? '' : '+'}${offset}`;
    };

    const handleParameterChange = (param) => {
        setSelectedParameter(param);
    };

    const handleDateChange = (e, type) => {
        if (type === 'start') {
            setStartDate(e.target.value);
        } else {
            setEndDate(e.target.value);
        }
    };

    const validateDates = () => {
        if (!startDate || !endDate) {
            setErrorMessage('Please select both start and end dates');
            return false;
        }

        const start = new Date(startDate);
        const end = new Date(endDate);

        if (start > end) {
            setErrorMessage('Start date must be less than or equal to end date');
            return false;
        }

        const diffTime = Math.abs(end - start);
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

        if (diffDays > 7) {
            setErrorMessage('The period between dates must be 7 days or less');
            return false;
        }

        setErrorMessage('');
        return true;
    };

    const handleShowChanges = async () => {
        if (validateDates()) {
            setIsLoading(true);
            try {
                const data = await weatherService.getWeathers(city.id, null, null, true, startDate, endDate);
                setWeatherChanges(data);
            } catch (error) {
                console.error("Failed to fetch weather changes:", error);
            } finally {
                setIsLoading(false);
            }
        }
    };

    const handleOverlayClick = (e) => {
        if (e.target.classList.contains('weather-details-modal-overlay')) {
            onClose();
        }
    };

    const formatDate = (dateString) => {
        if (!dateString) return '';
        return dateString.replace('T', ' ').replace('Z', '');
    };

    const filteredChanges = weatherChanges.map(change => ({
        date: formatDate(change.lastUpdated),
        temperatureCelsius: change.temperatureCelsius,
        humidity: change.humidity,
        pressureIn: change.pressureIn,
        windSpeedKph: change.windSpeedKph,
    }));

    const chartData = {
        labels: filteredChanges.map(change => change.date).reverse(),
        datasets: [
            {
                label: 'Temperature (°C)',
                data: filteredChanges.map(change => change.temperatureCelsius).reverse(),
                borderColor: 'rgba(75,192,192,1)',
                backgroundColor: 'rgba(75,192,192,0.2)',
                hidden: selectedParameter !== 'all' && selectedParameter !== 'temperature',
            },
            {
                label: 'Humidity (%)',
                data: filteredChanges.map(change => change.humidity).reverse(),
                borderColor: 'rgba(153,102,255,1)',
                backgroundColor: 'rgba(153,102,255,0.2)',
                hidden: selectedParameter !== 'all' && selectedParameter !== 'humidity',
            },
            {
                label: 'Pressure (mm Hg)',
                data: filteredChanges.map(change => change.pressureIn).reverse(),
                borderColor: 'rgba(255,159,64,1)',
                backgroundColor: 'rgba(255,159,64,0.2)',
                hidden: selectedParameter !== 'all' && selectedParameter !== 'pressure',
            },
            {
                label: 'Wind speed (km/h)',
                data: filteredChanges.map(change => change.windSpeedKph).reverse(),
                borderColor: 'rgba(54,162,235,1)',
                backgroundColor: 'rgba(54,162,235,0.2)',
                hidden: selectedParameter !== 'all' && selectedParameter !== 'windSpeed',
            },
        ],
    };

    const uniqueDates = [...new Set(filteredChanges.map(change => change.date.split(' ')[0]))];

    const chartOptions = {
        scales: {
            x: {
                ticks: {
                    callback: function (val, index) {
                        const date = new Date(this.getLabelForValue(val));
                        const formattedDate = date.toISOString().split('T')[0];
                        if (uniqueDates.includes(formattedDate) || (index % 24 === 0 && index !== 0)) {
                            return formattedDate;
                        }
                        return '';
                    },
                },
            },
        },
        plugins: {
            legend: {
                onClick: null
            }
        }
    };

    const renderCityInfo = () => {
        const { country, state, city: cityName } = city;
        let info = country;

        if (state) {
            info += `, ${state}`;
        }

        if (cityName) {
            info += `, ${cityName}`;
        }

        return info;
    };


    return (
        <div className="weather-details-modal-overlay" onClick={handleOverlayClick}>
            <div className="weather-details-modal">
                <div className="weather-details-modal-header">
                    <div className="weather-details-modal-header-left">
                        <h2>{renderCityInfo()}</h2>
                        <div><CurrentTime timeZone={city.tzId} /></div>
                    </div>
                    <button onClick={onClose} className="weather-details-modal-close-button">&times;</button>
                </div>

                {isLoading ? (
                     <Loader />
                ) : !weatherData ? (
                    <p className="no-weather">
                        Weather not found
                    </p>
                    ) : (
                        <>
                        <div className="weather-details-modal-view-selector">
                            <button className={view === 'current' ? 'active' : ''} onClick={() => setView('current')}>Current state</button>
                            <button className={view === 'changes' ? 'active' : ''} onClick={() => setView('changes')}>Last changes</button>
                        </div>
                        <div className="weather-details-modal-content">
                            {view === 'current' ? (
                                <div className="weather-details">
                                    <p><span>Weather conditions:</span> {weatherData.conditionText}</p>
                                    <p><span>Date of last update:</span> <i className="fa fa-clock-o"></i> {`${formatDate(weatherData.lastUpdated)} (UTC${getUtcOffsetForTimeZone(city.tzId)})`}</p>
                                    <div className="weather-details-modal-stats">
                                        <div>
                                            <p className='weather weather__temperatureCelsius'>Temperature</p>
                                            <p className='weather weather__temperatureCelsius'>{weatherData.temperatureCelsius}°C</p>
                                        </div>
                                        <div>
                                            <p className='weather weather__pressureIn'>Pressure</p>
                                            <p className='weather weather__pressureIn'>{weatherData.pressureIn} mm Hg</p>
                                        </div>
                                        <div>
                                            <p className='weather weather__humidity'>Humidity</p>
                                            <p className='weather weather__humidity'>{weatherData.humidity}%</p>
                                        </div>
                                        <div>
                                            <p className='weather weather__windSpeedKph'>Wind speed</p>
                                            <p className='weather weather__windSpeedKph'>{weatherData.windSpeedKph} km/h</p>
                                        </div>
                                    </div>
                                </div>
                            ) : (
                                <div className="weather-details-changes">
                                    {errorMessage && <p className="error-message error-message-weather-details">{errorMessage}</p>}
                                    <div className="weather-details-modal-date-picker">
                                        <span>Period</span>
                                        <input type="date" value={startDate} onChange={(e) => handleDateChange(e, 'start')} />
                                        <span>-</span>
                                        <input type="date" value={endDate} onChange={(e) => handleDateChange(e, 'end')} />
                                        <button className='button button__show' onClick={handleShowChanges}>Show</button>
                                    </div>
                                    <div className="weather-details-modal-parameter-selector">
                                        <button className={selectedParameter === 'all' ? 'active' : ''} onClick={() => handleParameterChange('all')}>All</button>
                                        <button className={selectedParameter === 'temperature' ? 'active' : ''} onClick={() => handleParameterChange('temperature')}>Temperature</button>
                                        <button className={selectedParameter === 'humidity' ? 'active' : ''} onClick={() => handleParameterChange('humidity')}>Humidity</button>
                                        <button className={selectedParameter === 'pressure' ? 'active' : ''} onClick={() => handleParameterChange('pressure')}>Pressure</button>
                                        <button className={selectedParameter === 'windSpeed' ? 'active' : ''} onClick={() => handleParameterChange('windSpeed')}>Wind speed</button>
                                    </div>
                                    <div className="weather-details-modal-flex-container">
                                        <div className="weather-details-modal-chart-container">
                                            <Line data={chartData} options={chartOptions} />
                                        </div>
                                        <WeatherChangesTable 
                                            weatherChanges={filteredChanges} 
                                            selectedParameter={selectedParameter}
                                        />
                                    </div>
                                </div>
                            )}
                        </div>
                        </>  
                    )
                }
                <div className="weather-details-modal-footer">
                    <button onClick={onClose} className="button button__modal button__close">Close</button>
                </div>  
            </div>
        </div>
    );
};

export default WeatherDetailsModal;
