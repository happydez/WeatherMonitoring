import React from 'react';
import WeatherListItem from '../appWeatherListItem/WeatherListItem';
import './weatherList.scss';

const WeatherList = ({ cities, editCity, deleteCity, isSearching, onEditCity, weatherService }) => {
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
                            editCity={editCity}
                            deleteCity={deleteCity}
                            onEditCity={onEditCity}
                            weatherService={weatherService}
                        />
                    ))}
                </ul>
            )}
        </div>
    );
};

export default WeatherList;
