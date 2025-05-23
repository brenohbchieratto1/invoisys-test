window.ordersModule = (function () {
    let currentPage = 1;
    let totalPages = 0;
    const pageSize = 10;
    let expandedOrderId = "";
    let expandedOrderDetails = "";
    let currentItems = [];

    function renderTable(items) {
        const tbody = $("#ordersTable tbody");
        tbody.empty();

        if (!items || items.length === 0) {
            tbody.append(`<tr><td colspan="5" class="text-center">Nenhum pedido encontrado.</td></tr>`);
            return;
        }

        items.forEach(order => {
            tbody.append(`
                <tr>
                    <td>${order.id}</td>
                    <td>${order.orderNumber}</td>
                    <td>${new Date(order.requestDate).toLocaleDateString()}</td>
                    <td>${new Date(order.estimatedDeliveryDate).toLocaleDateString()}</td>
                    <td>${order.orderNote}</td>
                    <td>
                        <button class="btn btn-link p-0" onclick="window.ordersModule.toggleDetails('${order.id}')" title="Ver detalhes">
                            <i class="bi bi-info-circle"></i>
                        </button>
                    </td>
                </tr>
            `);
            if (expandedOrderId === order.id && expandedOrderDetails) {
                const productsRows = expandedOrderDetails.products.map(p => `
                <tr>
                    <td>${p.productCode}</td>
                    <td>${p.productDescription}</td>
                    <td>${p.quantity}</td>
                    <td>${p.productPrice.toLocaleString('pt-BR', {style: 'currency', currency: 'BRL'})}</td>
                    <td>${(p.productPrice * p.quantity).toLocaleString('pt-BR', {style: 'currency', currency: 'BRL'})}</td>
                </tr>
            `).join("");

                tbody.append(`
                <tr class="table-active">
                    <td colspan="6">
                        <h6>Produtos do Pedido</h6>
                        <table class="table table-sm table-bordered mb-0">
                            <thead>
                                <tr>
                                    <th>Código</th>
                                    <th>Descrição</th>
                                    <th>Quantidade</th>
                                    <th>Valor Unitário</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${productsRows}
                            </tbody>
                        </table>
                    </td>
                </tr>
            `);
            }
        });
    }

    async function toggleDetails(orderId) {
        if (expandedOrderId === orderId) {
            expandedOrderId = null;
            expandedOrderDetails = null;
            renderTableCurrentPage();
        } else {
            await loadOrderDetails(orderId);
        }
    }

    function renderTableCurrentPage() {
        renderTable(currentItems);
    }

    async function loadOrderDetails(orderId) {
        const token = window.authModule.getTokenFromCache();
        if (!token) {
            alert("Token não encontrado no cache. Por favor, aguarde a autenticação.");
            return;
        }

        const correlationId = crypto.randomUUID();

        try {
            const response = await $.ajax({
                url: `/api/v1/orders/${orderId}`,
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "correlationId": correlationId,
                }
            });

            if (response.success && response.value) {
                expandedOrderId = orderId;
                expandedOrderDetails = response.value;
                renderTableCurrentPage();
            } else {
                alert("Não foi possível carregar os detalhes do pedido.");
            }
        } catch (error) {
            console.error(error);
            alert("Erro ao buscar detalhes do pedido.");
        }
    }

    async function loadPage(pageNumber) {
        const token = window.authModule.getTokenFromCache();
        if (!token) {
            alert("Token não encontrado no cache. Por favor, aguarde a autenticação.");
            return;
        }

        const correlationId = crypto.randomUUID();

        try {
            const response = await $.ajax({
                url: `/api/v1/orders?pageSize=${pageSize}&pageNumber=${pageNumber}`,
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "correlationId": `${correlationId}`,
                }
            });

            if (response.success && response.value) {
                currentPage = response.value.pageNumber;
                totalPages = response.value.totalPages;
                
                currentItems = response.value.items;
                renderTableCurrentPage();

                $("#pageInfo").text(`Página ${currentPage} de ${totalPages}`);

                $("#prevPage").prop("disabled", !response.value.hasPreviousPage);
                $("#nextPage").prop("disabled", !response.value.hasNextPage);
            } else {
                alert("Falha ao carregar pedidos.");
            }
        } catch {
            alert("Erro na requisição de pedidos.");
        }
    }

    async function createOrder(order) {
        const token = window.authModule.getTokenFromCache();
        if (!token) {
            alert("Token não encontrado no cache. Por favor, aguarde a autenticação.");
            return;
        }

        const correlationId = crypto.randomUUID();

        try {
            const response = await $.ajax({
                url: "/api/v1/orders",
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify(order),
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "correlationId": `${correlationId}`,
                }
            });

            if (response.success) {
                alert("Pedido criado com sucesso!");
                loadPage(currentPage, correlationId);
            } else {
                alert("Erro ao criar pedido.");
            }
        } catch (error) {
            console.error(error);
            alert("Erro na requisição ao criar pedido.");
        }
    }


    function initPagination() {
        $("#prevPage").off("click").on("click", function () {
            if (currentPage > 1) {
                loadPage(currentPage - 1);
            }
        });

        $("#nextPage").off("click").on("click", function () {
            if (currentPage < totalPages) {
                loadPage(currentPage + 1);
            }
        });
    }

    function initializeOrders() {
        loadPage(1);
        initPagination();
    }

    return {
        initializeOrders,
        loadPage,
        createOrder,
        toggleDetails,
    };
})();