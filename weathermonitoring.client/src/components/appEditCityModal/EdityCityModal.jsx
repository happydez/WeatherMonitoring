import React from 'react';
import './editCityModal.scss';

const EditCityModal = ({ city, onClose, onToggleTracking, weatherService  }) => {
    if (!city) return null;

    const handleToggleTracking = () => {
        weatherService.partiallyUpdateLocation(city.id, "replace", "/active", !city.tracking);
        onToggleTracking(city.id);
        onClose();
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

    const handleOverlayClick = (e) => {
        if (e.target.classList.contains('modal-overlay')) {
            onClose();
        }
    };

    return (
        <div className="modal-overlay" onClick={handleOverlayClick}>
            <div className="modal">
                <div className="modal-header">
                    <h2>{renderCityInfo()}</h2>
                    <button onClick={onClose} className="close-button">&times;</button>
                </div>
                <div className="modal-body">
                    <div className="modal-settings">
                        <h3>Settings</h3>
                        <button
                            onClick={handleToggleTracking}  
                            className={`${city.tracking ? 'button button__modal button__tracking button__tracking__active' : 'button button__modal button__tracking' }`}
                        >
                            {city.tracking ? "Disable Data Actualization" : "Enable Data Actualization"}
                        </button>
                    </div>
                </div>
                <div className="modal-footer">
                    <button onClick={onClose} className="button button__modal button__close">Close</button>
                </div>
            </div>
        </div>
    );
};

export default EditCityModal;
