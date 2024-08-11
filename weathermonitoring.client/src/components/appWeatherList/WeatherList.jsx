import React from 'react';
import WeatherListItem from '../appWeatherListItem/WeatherListItem';
import './weatherList.scss';

const WeatherList = ({ cities, deleteCity, isSearching, onEditCity, onShowWeatherDetails, weatherService  }) => {
    return (
        <div className="weather__list">
            {cities.length === 0 ? (
                <p className="no-cities">
                    {isSearching ? 'No locations found' : 'No locations added'}
                </p>
            ) : (
                <ul>
                    {cities.map((city, index) => (
                        <WeatherListItem
                            key={city.id}
                            index={index}
                            city={city}
                            deleteCity={deleteCity}
                            onEditCity={onEditCity}
                            onShowWeatherDetails={onShowWeatherDetails}
                            weatherService={weatherService}
                        />
                    ))}
                </ul>
            )}
        </div>
    );
};

export default WeatherList;
