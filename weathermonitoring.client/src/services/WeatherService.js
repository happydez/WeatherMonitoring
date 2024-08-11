class WeatherService {
    _port = 10000;
    _apiBase = `http://localhost:${this._port}/api/v1/`;

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
        return await this.getResource(`${this._apiBase}locations/search?q=${searchTerm}`);
    }

    getWeathers = async (locationId, limit = null, offset = null, desc = null, date1 = null, date2 = null) => {
        let url = `${this._apiBase}locations/${locationId}/weather?`;
        if (limit) url += `limit=${limit}&`;
        if (offset) url += `offset=${offset}&`;
        if (desc) url += `desc=${desc}&`;
        if (date1) url += `date1=${date1}T00:00:00Z&`;
        if (date2) url += `date2=${date2}T23:00:00Z&`;
        return await this.getResource(url);
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

    updateLocation = async (id, name, region, country, lat, lon, tzId, active, included) => {
        const body = {
            name,
            region,
            country,
            lat,
            lon,
            tzId,
            included,
            active
        };

        await this.updateResourse(`${this._apiBase}locations/${id}`, body);
    }

    partiallyUpdateResource = async (url, body) => {
        try {
            await fetch(url, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json-patch+json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(body),
            });
        } catch (error) {
            console.error(error);
        }
    }

    partiallyUpdateLocation = async (id, op, path, value) => {
        const body = [
            {
                op: op,
                path: path,
                value: `${value}`
            }
        ];

        await this.partiallyUpdateResource(`${this._apiBase}locations/${id}`, body);
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

    createLocation = async (name, region, country, lat, lon, tzId, active, included) => {
        const body = {
            name,
            region,
            country,
            lat,
            lon,
            tzId,
            included,
            active
        };

        return await this.createResourse(`${this._apiBase}locations`, body);
    }
}

export default WeatherService;
