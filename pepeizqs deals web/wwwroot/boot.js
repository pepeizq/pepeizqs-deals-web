(() => {
    const reintentosCantidad = 3;
    const reintentoMilisegundos = 5000;
    const reconnectModal = document.getElementById('reconnect-modal');

    const configureSignalR = () => {
        let c = builder.build();
        c.serverTimeoutInMilliseconds = 3000000;
        c.keepAliveIntervalInMilliseconds = 1500000;
        builder.build = () => {
            return c;
        };
    }

    const startReconnectionProcess = () => {
        reconnectModal.style.display = 'block';

        let isCanceled = false;

        (async () => {
            for (let i = 0; i < reintentosCantidad; i++) {
                reconnectModal.innerText = `Attempting to reconnect: ${i + 1} of ${reintentosCantidad}`;

                await new Promise(resolve => setTimeout(resolve, reintentoMilisegundos));

                if (isCanceled) {
                    return;
                }

                try
                {
                    const result = await Blazor.reconnect();

                    if (!result) {
                         location.reload();
                        return;
                    }

                    return;
                }
                catch { }
            }

            location.reload();
        })();

        return {
            cancel: () => {
                isCanceled = true;
                reconnectModal.style.display = 'none';
            },
        };
    };

    let currentReconnectionProcess = null;

    Blazor.start({
        circuit: {
            reconnectionHandler: {
                onConnectionDown: () => currentReconnectionProcess ??= startReconnectionProcess(),
                onConnectionUp: () => {
                    currentReconnectionProcess?.cancel();
                    currentReconnectionProcess = null;
                }
            }
        }
    });
})();