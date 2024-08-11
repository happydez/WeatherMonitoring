import React, { useState, useEffect, useCallback, useMemo } from 'react';
import AppFooter from "../appFooter/AppFooter";
import AppHeader from "../appHeader/AppHeader";
import WeatherList from "../appWeatherList/WeatherList";
import Pagination from "../appPagination/Pagination";
import AddCityModal from '../appAddCityModal/AddCityModal';
import EditCityModal from '../appEditCityModal/EdityCityModal';
import WeatherDetailsModal from '../appWeatherDetailsModal/WeatherDetailsModal';
import Loader from '../appLoader/Loader';

import WeatherService from '../../services/WeatherService';

import './app.scss';

const App = () => {
    const [cities, setCities] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [showAddModal, setShowAddModal] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedCity, setSelectedCity] = useState(null);
    const [showWeatherDetailsModal, setShowWeatherDetailsModal] = useState(false);
    const [showEditCityModal, setShowEditCityModal] = useState(false);
    const itemsPerPage = 5;

    const weatherService = useMemo(() => new WeatherService(), []);

    useEffect(() => {
        const fetchCities = async () => {
            try {
                const data = await weatherService.getAllIncludedLocations();
                const formattedCities = data.map(({ id, name, region, country, tzId, lat, lon, active }) => ({
                    id,
                    country,
                    state: region,
                    city: name,
                    tzId,
                    lat,
                    lon,
                    tracking: active,
                }));
                setCities(formattedCities);
            } catch (error) {
                console.error("Failed to fetch cities:", error);
            } finally {
                setIsLoading(false);
            }
        };

        fetchCities();
    }, [weatherService]);

    useEffect(() => {
        const totalPages = Math.max(Math.ceil(cities.length / itemsPerPage), 1);
        if (currentPage > totalPages) {
            setCurrentPage(totalPages);
        }
    }, [cities, currentPage, itemsPerPage]);

    const addCity = useCallback((city) => {
        setCities((prevCities) => [city, ...prevCities]);
    }, []);

    const deleteCity = useCallback((index) => {
        setCities((prevCities) => prevCities.filter((_, i) => i !== index));
    }, []);

    const toggleTracking = useCallback((id) => {
        setCities((prevCities) => prevCities.map(city =>
            city.id === id ? { ...city, tracking: !city.tracking } : city
        ));
    }, []);

    const searchCities = useCallback((term) => {
        if (term.length === 0) {
            return cities;
        }

        return cities.filter(city => {
            const fullLocation = `${city.country} ${city.state} ${city.city}`;
            return fullLocation.toLowerCase().includes(term.toLowerCase());
        });
    }, [cities]);

    const filteredCities = searchCities(searchTerm);

    const indexOfLastCity = currentPage * itemsPerPage;
    const indexOfFirstCity = indexOfLastCity - itemsPerPage;
    const currentCities = filteredCities.slice(indexOfFirstCity, indexOfLastCity);

    const paginate = (pageNumber) => setCurrentPage(pageNumber);

    const onShowWeatherDetails = (city) => {
        setSelectedCity(city);
        setShowWeatherDetailsModal(true);
        setShowEditCityModal(false);
    };

    const onEditCity = (city) => {
        setSelectedCity(city);
        setShowEditCityModal(true);
        setShowWeatherDetailsModal(false);
    };

    if (isLoading) {
        return (
            <div className="loader-container">
                <Loader />
            </div>
        );
    }

    return (
        <div className="app">
            <AppHeader onAddCity={() => setShowAddModal(true)} onSearch={setSearchTerm} />
            <main className="main-content">
                <div className="weather__content">
                    <WeatherList 
                        cities={currentCities}
                        deleteCity={deleteCity}
                        isSearching={searchTerm.length > 0}
                        onEditCity={onEditCity}
                        onShowWeatherDetails={onShowWeatherDetails}
                        weatherService={weatherService}
                    />
                </div>
                <section className="pagination-container">
                    {filteredCities.length > itemsPerPage && (
                        <Pagination 
                            totalItems={filteredCities.length}
                            itemsPerPage={itemsPerPage}
                            currentPage={currentPage}
                            paginate={paginate}
                        />
                    )}
                </section>
            </main>
            <AppFooter />
            {showAddModal && (
                <AddCityModal
                    onHide={() => setShowAddModal(false)}
                    onAddCity={(city) => {
                        addCity(city);
                        setShowAddModal(false);
                    }}
                    existingCities={cities}
                    weatherService={weatherService}
                />
            )}
            {showEditCityModal && selectedCity && (
                <EditCityModal
                    city={selectedCity}
                    onClose={() => setShowEditCityModal(false)}
                    onToggleTracking={toggleTracking}
                    weatherService={weatherService}
                />
            )}
            {showWeatherDetailsModal && selectedCity && (
                <WeatherDetailsModal
                    city={selectedCity}
                    onClose={() => setShowWeatherDetailsModal(false)}
                    weatherService={weatherService}
                />
            )}
        </div>
    );
}

export default App;
