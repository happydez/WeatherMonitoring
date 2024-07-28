import React from 'react';
import './weatherListItem.scss';

const WeatherListItem = ({ index, city, deleteCity, onEditCity, weatherService }) => {
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

    const onDeleteCity = () => {
        weatherService.updateLocation(city.id, false, false);
        deleteCity(index);
    };

    return (
        <li className="weather__item">
            <div className="weather__info">
                <div className="weather__city">
                    {renderCityInfo()}
                </div>
            </div>
            <div className="weather__control">
                <div className={`${city.tracking ? 'weather__tracking weather__tracking__active' : 'weather__tracking'}`}>
                    {city.tracking ? "tracking enabled" : "not tracking"}
                </div>
                <button 
                    className='control control__settings' 
                    onClick={() => onEditCity(city)}
                >
                    <i className="fa fa-wrench"></i>
                </button>
                <button className='control control__remove' onClick={onDeleteCity}>
                    <i className="fa fa-trash"></i>
                </button>
            </div>
        </li>
    )
}

export default WeatherListItem;
