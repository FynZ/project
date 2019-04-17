FROM microsoft/dotnet:2.2-aspnetcore-runtime
LABEL org.label-schema.vcs-url="https://git.vpgrp.io/supply-chain/lni"
LABEL org.label-schema.vendor="vp.dsi.supply"
LABEL org.label-schema.name="lni"
ENV ASPNETCORE_ENVIRONMENT="${EnvironmentName}"
ARG TARGET_VERSION
ARG source
WORKDIR /app
COPY . /app
EXPOSE 80
CMD ["dotnet", "Monsters.dll"]