window.uiService = (() => {
    const toggleLoading = (show) => {
        document.getElementById('loading-overlay').classList.toggle('hidden', !show);
    };

    const showToast = (message, type = 'success') => {
        const toast = document.getElementById('toast');
        const toastMessage = document.getElementById('toast-message');
        toastMessage.textContent = message;
        toast.className = `fixed bottom-5 right-5 px-6 py-3 text-white rounded-lg shadow-lg transition-all duration-300 transform translate-y-16 ${type === 'success' ? 'bg-green-500' : 'bg-red-600'}`;
        toast.classList.remove('hidden');
        setTimeout(() => toast.classList.remove('translate-y-16'), 10);
        setTimeout(() => {
            toast.classList.add('translate-y-16');
            setTimeout(() => toast.classList.add('hidden'), 300);
        }, 4000);
    };

    const renderAnalytics = (data = {}) => {
        const grid = document.getElementById('analytics-grid');
        grid.innerHTML = ''; // Clear previous data

        const stats = [
            { label: 'Total Keys', value: data.totalKeys || 0, icon: 'key-round' },
            { label: 'Active Keys', value: data.keysStatus?.totalActiveKeys || 0, icon: 'shield-check' },
            { label: 'Revoked Keys', value: data.keysStatus?.totalRevokedKeys || 0, icon: 'shield-off' },
            { label: 'Total Users', value: data.keysPerUser?.length || 0, icon: 'users' }
        ];

        stats.forEach(stat => {
            const card = document.createElement('div');
            card.className = 'bg-white p-5 rounded-lg shadow-md flex items-center space-x-4';
            card.innerHTML = `
                <div class="bg-blue-100 p-3 rounded-full"><i data-lucide="${stat.icon}" class="w-6 h-6 text-blue-600"></i></div>
                <div><p class="text-sm text-gray-500">${stat.label}</p><p class="text-2xl font-bold text-gray-900">${stat.value}</p></div>`;
            grid.appendChild(card);
        });
        
        const createListCard = (title, items, renderFn) => {
            const card = document.createElement('div');
            card.className = 'bg-white p-5 rounded-lg shadow-md md:col-span-2';
            let content = `<h4 class="text-md font-semibold text-gray-800 mb-3">${title}</h4>`;
            if (items && items.length > 0) {
                content += `<ul class="space-y-2">${items.map(renderFn).join('')}</ul>`;
            } else {
                content += `<p class="text-sm text-gray-500">No data available.</p>`;
            }
            card.innerHTML = content;
            return card;
        };
        
        // Enhanced Top Used Keys card
        const topUsedKeysCard = document.createElement('div');
        topUsedKeysCard.className = 'bg-white p-6 rounded-lg shadow-md md:col-span-2 border-l-4 border-green-500';
        let topUsedContent = `
            <div class="flex items-center mb-4">
                <div class="bg-green-100 p-2 rounded-full mr-3">
                    <i data-lucide="trending-up" class="w-5 h-5 text-green-600"></i>
                </div>
                <h4 class="text-lg font-semibold text-gray-800">Top Used Keys</h4>
            </div>`;
        
        if (data.topUsedKeys && data.topUsedKeys.length > 0) {
            topUsedContent += `<div class="space-y-3">`;
            data.topUsedKeys.forEach((key, index) => {
                const rankColor = index === 0 ? 'text-yellow-600 bg-yellow-100' : index === 1 ? 'text-gray-600 bg-gray-100' : 'text-orange-600 bg-orange-100';
                const rankIcon = index === 0 ? 'crown' : index === 1 ? 'medal' : 'award';
                topUsedContent += `
                    <div class="flex items-center justify-between p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors">
                        <div class="flex items-center space-x-3">
                            <div class="${rankColor} p-1.5 rounded-full">
                                <i data-lucide="${rankIcon}" class="w-4 h-4"></i>
                            </div>
                            <div>
                                <p class="font-medium text-gray-900 truncate max-w-48">${key.keyName}</p>
                                <p class="text-xs text-gray-500">Rank #${index + 1}</p>
                            </div>
                        </div>
                        <div class="text-right">
                            <span class="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-blue-100 text-blue-800">
                                <i data-lucide="activity" class="w-3 h-3 mr-1"></i>
                                ${key.usage} uses
                            </span>
                        </div>
                    </div>`;
            });
            topUsedContent += `</div>`;
        } else {
            topUsedContent += `
                <div class="text-center py-8">
                    <i data-lucide="bar-chart-3" class="w-12 h-12 text-gray-300 mx-auto mb-3"></i>
                    <p class="text-gray-500">No usage data available yet</p>
                    <p class="text-xs text-gray-400 mt-1">Keys will appear here once they start being used</p>
                </div>`;
        }
        topUsedKeysCard.innerHTML = topUsedContent;
        grid.appendChild(topUsedKeysCard);
        
        // Enhanced Idle Keys card
        const idleKeysCard = document.createElement('div');
        idleKeysCard.className = 'bg-white p-6 rounded-lg shadow-md md:col-span-2 border-l-4 border-orange-500';
        let idleContent = `
            <div class="flex items-center mb-4">
                <div class="bg-orange-100 p-2 rounded-full mr-3">
                    <i data-lucide="clock" class="w-5 h-5 text-orange-600"></i>
                </div>
                <h4 class="text-lg font-semibold text-gray-800">Idle Keys</h4>
            </div>`;
        
        if (data.idleKeys && data.idleKeys.length > 0) {
            idleContent += `<div class="grid grid-cols-1 sm:grid-cols-2 gap-2">`;
            data.idleKeys.forEach(key => {
                idleContent += `
                    <div class="flex items-center p-3 bg-orange-50 rounded-lg border border-orange-100">
                        <div class="bg-orange-200 p-1.5 rounded-full mr-3">
                            <i data-lucide="pause-circle" class="w-4 h-4 text-orange-600"></i>
                        </div>
                        <div class="flex-1">
                            <p class="font-medium text-gray-900 truncate">${key.keyName}</p>
                            <p class="text-xs text-orange-600">No recent activity</p>
                        </div>
                    </div>`;
            });
            idleContent += `</div>`;
        } else {
            idleContent += `
                <div class="text-center py-8">
                    <i data-lucide="zap" class="w-12 h-12 text-gray-300 mx-auto mb-3"></i>
                    <p class="text-gray-500">All keys are active!</p>
                    <p class="text-xs text-gray-400 mt-1">Great job keeping your API keys busy</p>
                </div>`;
        }
        idleKeysCard.innerHTML = idleContent;
        grid.appendChild(idleKeysCard);

        lucide.createIcons();
    };

    const renderKeysTable = (keys) => {
        const tableBody = document.getElementById('keys-table-body');
        tableBody.innerHTML = '';
        if (!keys || keys.length === 0) {
            tableBody.innerHTML = `<tr><td colspan="6" class="text-center py-12 text-gray-500">No keys found.</td></tr>`;
            return;
        }
        keys.forEach(key => {
            const statusClass = key.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800';
            const revokedDate = key.revokedAt ? `<span class="text-red-600 font-semibold">${new Date(key.revokedAt).toLocaleDateString()}</span>` : 'N/A';
            const row = document.createElement('tr');
            row.dataset.keyId = key.id;
            row.innerHTML = `
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${key.keyName}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    <div class="flex items-center"><span>****************</span>
                        <button data-action="show-key" data-key-value="${key.keyValue}" class="ml-2 p-1 text-gray-400 hover:text-blue-600"><i data-lucide="eye" class="w-4 h-4 pointer-events-none"></i></button>
                        <button data-action="copy-key" data-key-value="${key.keyValue}" class="ml-1 p-1 text-gray-400 hover:text-blue-600"><i data-lucide="copy" class="w-4 h-4 pointer-events-none"></i></button>
                    </div></td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${key.quota}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm"><span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${statusClass}">${key.status}</span></td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${revokedDate}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium"><div class="flex items-center space-x-2">
                    <button data-action="verify-key" data-key-value="${key.keyValue}" class="text-blue-600 hover:text-blue-900" title="Verify Key"><i data-lucide="check-circle" class="w-5 h-5 pointer-events-none"></i></button>
                    <button data-action="edit-quota" class="text-indigo-600 hover:text-indigo-900" title="Edit Quota"><i data-lucide="edit" class="w-5 h-5 pointer-events-none"></i></button>
                    ${key.status === 'Active' ? `<button data-action="revoke-key" class="text-red-600 hover:text-red-900" title="Revoke Key"><i data-lucide="trash-2" class="w-5 h-5 pointer-events-none"></i></button>` : ''}
                </div></td>`;
            tableBody.appendChild(row);
        });
        lucide.createIcons();
    };

    const renderPagination = (currentPage, totalPages) => {
        const container = document.getElementById('pagination-controls');
        container.innerHTML = '';
        if (totalPages <= 1) return;
        let paginationHtml = '<nav class="relative z-0 inline-flex rounded-md shadow-sm -space-x-px" aria-label="Pagination">';
        for (let i = 1; i <= totalPages; i++) {
            const isCurrent = i === currentPage;
            paginationHtml += `<button data-action="change-page" data-page-number="${i}" class="${isCurrent ? 'z-10 bg-blue-50 border-blue-500 text-blue-600' : 'bg-white border-gray-300 text-gray-500 hover:bg-gray-50'} relative inline-flex items-center px-4 py-2 border text-sm font-medium">${i}</button>`;
        }
        paginationHtml += '</nav>';
        container.innerHTML = paginationHtml;
    };
    
    const openModal = (htmlContent) => {
        const modalContainer = document.getElementById('modal-container');
        modalContainer.innerHTML = `<div class="modal-overlay"><div class="modal-content">${htmlContent}</div></div>`;
        const overlay = modalContainer.querySelector('.modal-overlay');
        setTimeout(() => overlay.classList.add('active'), 10);
        lucide.createIcons();
    };

    const closeModal = () => {
        const modalContainer = document.getElementById('modal-container');
        const overlay = modalContainer.querySelector('.modal-overlay');
        if (overlay) {
            overlay.classList.remove('active');
            setTimeout(() => modalContainer.innerHTML = '', 300);
        }
    };

    return { toggleLoading, showToast, renderAnalytics, renderKeysTable, renderPagination, openModal, closeModal };
})();
