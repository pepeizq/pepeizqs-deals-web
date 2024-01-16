(() => {
    const reintentos = 5;
    const reintentoMilisegundos = 3000;

    const startReconnectionProcess = () => {
        let cancelado = false;

        (async () => {
            for (let i = 0; i < reintentos; i++) {
                await new Promise(resolve => setTimeout(resolve, reintentoMilisegundos));

                if (cancelado) {
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
                cancelado = true;
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
        },
        configureSignalR: function (builder) {
          let c = builder.build();
          c.serverTimeoutInMilliseconds = 3000000;
          c.keepAliveIntervalInMilliseconds = 1500000;
          builder.build = () => {
            return c;
          };
        }
    });
})();