class WeatherService {
    _port = 7120;
    _apiBase = process.env.REACT_APP_CLIENT_API_BASE || `http://localhost:${this._port}/api/v1/`;

    getResource = async (url) => {
        let response = await fetch(url);

        if (!response.ok) {
            throw new Error(`Could not fetch ${url}, status: ${response.status}`);
        }

        return await response.json();
    }

    getAllIncludedLocations = async () => {
        return await this.getResource(`${this._apiBase}locations?included=true`);
    }

    searchLocation = async(searchTerm) => {
        return await this.getResource(`${this._apiBase}search?q=${searchTerm}`);
    }

    updateResourse = async (url, body) => {
        try {
            await fetch(url, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(body),
            });
        } catch (error) {
            console.error(error);
        }
    }

    updateLocation = async (id, included, active) => {
        const body = {
            included: included,
            active: active,
        };

        await this.updateResourse(`${this._apiBase}locations/${id}`, body);
    }

    createResourse = async (url, body) => {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        });

        if (!response.ok) {
            throw new Error(`Could not fetch ${url}, status: ${response.status}`);
        }

        return await response.json();
    }

    createLocation = async (body) => {
        return await this.createResourse(`${this._apiBase}locations`, body);
    }
}

export default WeatherService;
