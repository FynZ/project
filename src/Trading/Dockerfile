FROM microsoft/dotnet:2.2-aspnetcore-runtime
LABEL org.label-schema.vcs-url="https://github.com/FynZ/project"
LABEL org.label-schema.vendor="com.fynzie"
LABEL org.label-schema.name="pocker-monsters"
ENV ASPNETCORE_ENVIRONMENT="${EnvironmentName}"
ARG TARGET_VERSION
ARG source
WORKDIR /app
COPY . /app
EXPOSE 80
CMD ["dotnet", "Trading.dll"]