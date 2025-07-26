window.apiService = (() => {
    const API_BASE_URL = 'http://localhost:63768/api';

    async function fetchWithAuth(endpoint, token, options = {}) {
        const headers = {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
            ...options.headers,
        };

        try {
            console.log(`Making API call to: ${API_BASE_URL}${endpoint}`);
            console.log('Headers:', headers);
            const response = await fetch(`${API_BASE_URL}${endpoint}`, { ...options, headers });
            
            console.log(`Response status: ${response.status}`);
            
            if (response.status === 204) {
                console.log('Received 204 No Content response');
                return null; // Handle No Content responses gracefully
            }

            const result = await response.json();
            console.log('API Response:', result);

            if (!response.ok || !result.metaData?.succeeded) {
                const errorMessage = result.metaData?.errors?.[0] || `API Error: ${response.status}`;
                console.error('API Error:', errorMessage);
                throw new Error(errorMessage);
            }
            
            return result.data;

        } catch (error) {
            console.error('Fetch error:', error);
            // Re-throw the error to be caught by the calling function
            throw error;
        }
    }

    return {
        getUsage: (token) => fetchWithAuth('/Key/usage', token),
        getKeys: (token, pageNumber = 1, pageSize = 10) => fetchWithAuth(`/Key/list?PageSize=${pageSize}&PageNumber=${pageNumber}`, token),
        createKey: (token, body) => fetchWithAuth('/Key/create', token, { method: 'POST', body: JSON.stringify(body) }),
        revokeKey: (token, body) => fetchWithAuth('/Key/revoke', token, { method: 'PUT', body: JSON.stringify(body) }),
        updateQuota: (token, body) => fetchWithAuth('/Key/quota-update', token, { method: 'PUT', body: JSON.stringify(body) }),
        verifyKey: (token, body) => fetchWithAuth('/Key/verify', token, { method: 'POST', body: JSON.stringify(body) }),
    };
})();
