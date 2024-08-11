import React from 'react';
import './appHeader.scss';

const AppHeader = ({ onAddCity, onSearch }) => {
    const handleSearchChange = (e) => {
        onSearch(e.target.value);
    };

    return (
        <header>
            <h1 className="title">
                <button className="link-button">
                    WeatherMonitoring
                </button>
            </h1>
            <div className="btns">
                <button className="button button__search">
                    <input 
                        type="text" 
                        placeholder='Search Location' 
                        onChange={handleSearchChange} 
                    />
                    <i className="fa fa-search"></i>
                </button>
                <button className="button button__add" onClick={onAddCity}>
                    <div>Add location</div>
                </button>
            </div>
        </header>
    );
}

export default AppHeader;
