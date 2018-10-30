#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 65325
EXPOSE 44397

FROM microsoft/dotnet:2.1-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["InspectorUsageSample/InspectorUsageSample.csproj", "InspectorUsageSample/"]
RUN dotnet restore "InspectorUsageSample/InspectorUsageSample.csproj"
COPY . .
WORKDIR "/src/InspectorUsageSample"
RUN dotnet build "InspectorUsageSample.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "InspectorUsageSample.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "InspectorUsageSample.dll"]