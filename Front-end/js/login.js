document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('login-form');
    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');
    const loginButton = document.getElementById('login-button');
    const buttonText = loginButton.querySelector('.button-text');
    const buttonLoader = loginButton.querySelector('.button-loader');

    const showToast = (message, type = 'error') => {
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

    const setLoadingState = (isLoading) => {
        loginButton.disabled = isLoading;
        buttonText.classList.toggle('hidden', isLoading);
        buttonLoader.classList.toggle('hidden', !isLoading);
    };

    const validateInput = (input, errorElement, message) => {
        if (input.value.trim() === '') {
            input.classList.add('border-red-500');
            errorElement.textContent = message;
            errorElement.classList.remove('hidden');
            return false;
        }
        input.classList.remove('border-red-500');
        errorElement.classList.add('hidden');
        return true;
    };

    loginForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        const isUsernameValid = validateInput(usernameInput, document.getElementById('username-error'), 'Username is required.');
        const isPasswordValid = validateInput(passwordInput, document.getElementById('password-error'), 'Password is required.');

        if (!isUsernameValid || !isPasswordValid) return;

        setLoadingState(true);

        try {
            const response = await fetch('http://localhost:63768/api/Users/Login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    username: usernameInput.value,
                    password: passwordInput.value,
                }),
            });

            const result = await response.json();

            if (result.metaData?.succeeded) {
                sessionStorage.setItem('authToken', result.data.token);
                showToast('Login successful!', 'success');
                setTimeout(() => {
                    window.location.href = 'dashboard.html';
                }, 1000);
            } else {
                const errorMessage = result.metaData?.errors?.[0] || 'An unknown error occurred.';
                showToast(errorMessage);
                setLoadingState(false);
            }
        } catch (error) {
            console.error('Login request failed:', error);
            showToast('Could not connect to the server. Please try again later.');
            setLoadingState(false);
        }
    });
});
