window.authModule = (function () {
    const tokenKey = "auth_Token";

    async function initializeAuth() {
        if (localStorage.getItem(tokenKey)) {
            return;
        }
        
        try {
            const response = await $.ajax({
                url: "/api/v1/authentication",
                method: "POST"
            });

            if (response && response.token) {
                localStorage.setItem(tokenKey, response.token);
            } else {
                console.error("Falha ao obter token na autenticação");
            }
        } catch (error) {
            console.error("Erro na requisição de autenticação:", error);
        }
    }

    function getTokenFromCache() {
        return localStorage.getItem(tokenKey);
    }

    return {
        initializeAuth,
        getTokenFromCache
    };
})();