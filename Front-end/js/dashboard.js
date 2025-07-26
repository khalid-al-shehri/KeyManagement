document.addEventListener('DOMContentLoaded', () => {
    const state = {
        token: null,
        keys: [],
        currentPage: 1,
        totalPages: 1,
        isFetching: false,
    };

    const api = window.apiService;
    const ui = window.uiService;

    async function loadDashboard() {
        if (state.isFetching) return;
        state.isFetching = true;
        ui.toggleLoading(true);
        try {
            const [analyticsData, keysData] = await Promise.all([
                api.getUsage(state.token),
                api.getKeys(state.token, state.currentPage)
            ]);
            
            state.keys = keysData?.listData || [];
            state.totalPages = Math.ceil((keysData?.totalCount || 0) / (keysData?.pageSize || 10));

            ui.renderAnalytics(analyticsData);
            ui.renderKeysTable(state.keys);
            ui.renderPagination(state.currentPage, state.totalPages);
        } catch (error) {
            handleApiError(error);
        } finally {
            state.isFetching = false;
            ui.toggleLoading(false);
        }
    }

    async function refreshKeys(pageNumber) {
        if (state.isFetching) return;
        state.isFetching = true;
        ui.toggleLoading(true);
        try {
            state.currentPage = pageNumber;
            const keysData = await api.getKeys(state.token, state.currentPage);
            state.keys = keysData?.listData || [];
            state.totalPages = Math.ceil((keysData?.totalCount || 0) / (keysData?.pageSize || 10));
            ui.renderKeysTable(state.keys);
            ui.renderPagination(state.currentPage, state.totalPages);
        } catch (error) {
            handleApiError(error);
        } finally {
            state.isFetching = false;
            ui.toggleLoading(false);
        }
    }

    function handleApiError(error) {
        console.error("API Error:", error);
        ui.showToast(error.message, 'error');
        if (String(error.message).includes('401') || error.message.includes('Unauthorized')) {
            handleLogout();
        }
    }

    function handleLogout() {
        sessionStorage.removeItem('authToken');
        window.location.href = 'index.html';
    }

    function init() {
        state.token = sessionStorage.getItem('authToken');
        if (!state.token) {
            handleLogout();
            return;
        }
        document.body.addEventListener('click', handleActionDelegation);
        document.body.addEventListener('submit', handleFormSubmission);
        loadDashboard();
    }

    // --- Action Handlers ---
    const actions = {
        logout: handleLogout,
        'change-page': (el) => refreshKeys(parseInt(el.dataset.pageNumber, 10)),
        'copy-key': (el) => {
            navigator.clipboard.writeText(el.dataset.keyValue)
                .then(() => ui.showToast('Key copied!', 'success'))
                .catch(() => ui.showToast('Failed to copy.', 'error'));
        },
        'show-key': (el) => {
            ui.openModal(`
                <h3 class="text-lg font-medium text-gray-900">API Key Value</h3>
                <div class="mt-4 bg-gray-100 p-3 rounded-md"><p class="text-sm text-gray-700 break-all">${el.dataset.keyValue}</p></div>
                <div class="mt-6 text-right"><button data-action="close-modal" class="px-4 py-2 bg-gray-200 text-gray-800 rounded-md hover:bg-gray-300">Close</button></div>`);
        },
        'verify-key': async (el) => {
            ui.toggleLoading(true);
            try {
                const result = await api.verifyKey(state.token, { keyValue: el.dataset.keyValue });
                ui.showToast(result.message, 'success');
            } catch (error) { handleApiError(error); } 
            finally { ui.toggleLoading(false); }
        },
        'revoke-key': (el) => {
            const key = state.keys.find(k => k.id === el.closest('tr').dataset.keyId);
            if (!key) return;
            ui.openModal(`
                <h3 class="text-lg font-medium text-gray-900">Confirm Revoke</h3>
                <p class="mt-2 text-sm text-gray-600">Are you sure you want to revoke the key "${key.keyName}"? This action is permanent.</p>
                <div class="mt-6 flex justify-end space-x-3">
                    <button data-action="close-modal" class="px-4 py-2 bg-gray-200 text-gray-800 rounded-md hover:bg-gray-300">Cancel</button>
                    <button data-action="confirm-revoke" data-key-id="${key.id}" class="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700">Confirm Revoke</button>
                </div>`);
        },
        'confirm-revoke': async (el) => {
            ui.toggleLoading(true);
            try {
                await api.revokeKey(state.token, { id: el.dataset.keyId });
                ui.showToast('Key revoked successfully.', 'success');
                ui.closeModal();
                await loadDashboard();
            } catch (error) { handleApiError(error); } 
            finally { ui.toggleLoading(false); }
        },
        'edit-quota': (el) => {
            const key = state.keys.find(k => k.id === el.closest('tr').dataset.keyId);
            if (!key) return;
            ui.openModal(`
                <h3 class="text-lg font-medium text-gray-900">Edit Quota</h3>
                <form id="edit-quota-form" data-key-id="${key.id}" class="mt-4 space-y-4">
                    <div><label class="block text-sm font-medium text-gray-700">Key Name</label><input type="text" value="${key.keyName}" class="mt-1 w-full bg-gray-200 p-2 rounded-md" disabled></div>
                    <div><label for="new-quota" class="block text-sm font-medium text-gray-700">New Quota</label><input type="number" id="new-quota" value="${key.quota}" min="0" required class="mt-1 w-full border-gray-300 p-2 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"></div>
                    <div class="mt-6 flex justify-end space-x-3">
                        <button type="button" data-action="close-modal" class="px-4 py-2 bg-gray-200 text-gray-800 rounded-md hover:bg-gray-300">Cancel</button>
                        <button type="submit" class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700">Update</button>
                    </div>
                </form>`);
        },
        'create-key': () => {
             ui.openModal(`
                <h3 class="text-lg font-medium text-gray-900">Create New Key</h3>
                <form id="create-key-form" class="mt-4 space-y-4">
                    <div><label for="new-key-name" class="block text-sm font-medium text-gray-700">Key Name</label><input type="text" id="new-key-name" required class="mt-1 w-full border-gray-300 p-2 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"></div>
                    <div><label for="new-key-quota" class="block text-sm font-medium text-gray-700">Quota</label><input type="number" id="new-key-quota" required min="1" class="mt-1 w-full border-gray-300 p-2 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"></div>
                    <div class="mt-6 flex justify-end space-x-3">
                        <button type="button" data-action="close-modal" class="px-4 py-2 bg-gray-200 text-gray-800 rounded-md hover:bg-gray-300">Cancel</button>
                        <button type="submit" class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700">Create</button>
                    </div>
                </form>`);
        },
        'close-modal': ui.closeModal,
    };

    function handleActionDelegation(event) {
        const actionElement = event.target.closest('[data-action]');
        if (actionElement) {
            const action = actionElement.dataset.action;
            if (actions[action]) {
                actions[action](actionElement);
            }
        }
    }

    const formHandlers = {
        'create-key-form': async (form) => {
            const keyName = form.querySelector('#new-key-name').value;
            const quota = form.querySelector('#new-key-quota').value;
            await api.createKey(state.token, { keyName, quota: parseInt(quota, 10) });
            ui.showToast('Key created successfully.', 'success');
            ui.closeModal();
            await loadDashboard();
        },
        'edit-quota-form': async (form) => {
            const keyId = form.dataset.keyId;
            const newQuota = form.querySelector('#new-quota').value;
            await api.updateQuota(state.token, { id: keyId, quota: parseInt(newQuota, 10) });
            ui.showToast('Quota updated successfully.', 'success');
            ui.closeModal();
            await refreshKeys(state.currentPage);
        },
    };

    async function handleFormSubmission(event) {
        if (event.target.tagName === 'FORM') {
            event.preventDefault();
            const handler = formHandlers[event.target.id];
            if (handler) {
                ui.toggleLoading(true);
                try {
                    await handler(event.target);
                } catch (error) {
                    handleApiError(error);
                } finally {
                    ui.toggleLoading(false);
                }
            }
        }
    }

    init();
});
