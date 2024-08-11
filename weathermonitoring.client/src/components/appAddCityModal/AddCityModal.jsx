import React, { useState } from 'react';
import './addCityModal.scss';
import Loader from '../appLoader/Loader';

const AddCityModal = ({ onHide, onAddCity, existingCities, weatherService }) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [selectedLocation, setSelectedLocation] = useState(null);
  const [isDuplicate, setIsDuplicate] = useState(false);
  const [isTrackingEnabled, setIsTrackingEnabled] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [isSearching, setIsSearching] = useState(false);

  const handleSearch = async () => {
    setIsSearching(true);
    try {
      const data = await weatherService.searchLocation(searchTerm);
      setSearchResults(data);
      setSelectedLocation(null);

      if (data.length === 0) {
        setErrorMessage('No locations found');
      } else {
        setErrorMessage('');
      }
    } catch (error) {
      setErrorMessage('Error searching for locations');
    } finally {
      setIsSearching(false);
    }
  };

  const handleAddCity = async () => {
    if (selectedLocation) {
      const body = {
        name: selectedLocation.name,
        region: selectedLocation.region,
        country: selectedLocation.country,
        lat: selectedLocation.lat,
        lon: selectedLocation.lon,
        tzId: "Etc/UTC",
        active: isTrackingEnabled,
        included: true,
      };

      const duplicate = existingCities.some(
        city =>
          city.country === body.country &&
          city.state === body.region &&
          city.city === body.name
      );

      if (!duplicate) {
          try {
              const response = await weatherService.createLocation(
                  body.name,
                  body.region,
                  body.country,
                  body.lat,
                  body.lon,
                  body.tzId,
                  body.active,
                  body.included
              );
          
          const newCity = {
            id: response.id,
            country: response.country,
            state: response.region,
            city: response.name,
            tzId: response.tzId,
            lat: response.lat,
            lon: response.lon,
            tracking: response.active,
          };

          onAddCity(newCity);
          handleClose();
        } catch (error) {
          console.error("Failed to create location:", error);
          setErrorMessage('Failed to create location');
        }
      } else {
        setIsDuplicate(true);
      }
    }
  };

  const handleClose = () => {
    setSearchTerm('');
    setSearchResults([]);
    setSelectedLocation(null);
    setIsDuplicate(false);
    setIsTrackingEnabled(false);
    setErrorMessage('');
    onHide();
  };

  const handleOverlayClick = (e) => {
    if (e.target.classList.contains('modal-overlay')) {
      handleClose();
    }
  };

  return (
    <div className="modal-overlay" onClick={handleOverlayClick}>
      <div className="modal">
        <div className="modal-header">
          <h2>Add Location</h2>
          <button onClick={handleClose} className="close-button">&times;</button>
        </div>
        <div className="modal-body">
          {isDuplicate && <p className="error-message">This location already exists in the list</p>}
          {errorMessage && <p className="error-message">{errorMessage}</p>}
          <div className="search-container">
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => {
                if (e.target.value.length > 32) return;

                setSearchTerm(e.target.value);
                setSearchResults([]);
                setSelectedLocation(null);
              }}
              placeholder="Enter city name"
            />
            <button
              onClick={handleSearch}
              className="button button__search button__innersearch"
              disabled={!searchTerm.trim() || isSearching || searchResults.length > 0}
            >
              Search
            </button>

            {isSearching && (
              <div className='modal-loader'>
                <Loader />
              </div>
            )}

          </div>
          {searchResults.length > 0 && (
            <select
              onChange={(e) => {
                const location = JSON.parse(e.target.value);
                setSelectedLocation(location);
                setIsDuplicate(false);
              }}
              defaultValue=""
            >
              <option value="" disabled>Select a location</option>
              {searchResults.map((location, index) => (
                <option key={index} value={JSON.stringify(location)}>
                  {`${location.country}, ${location.region}, ${location.name}`}
                </option>
              ))}
            </select>
          )}
        </div>
        <div className="modal-footer">
          <button onClick={handleClose} className="button button__modal button__close">Close</button>
          <button
            onClick={handleAddCity}
            className="button button__modal button__apply"
            disabled={!selectedLocation}
          >
            Add Location
          </button>
          <button
            onClick={() => setIsTrackingEnabled(!isTrackingEnabled)}
            className={`${isTrackingEnabled ? 'button button__modal button__tracking button__tracking__active' : 'button button__modal button__tracking'}`}
          >
            {isTrackingEnabled ? "Disable Data Actualization" : "Enable Data Actualization"}
          </button>
        </div>
      </div>
    </div>
  );
};

export default AddCityModal;
