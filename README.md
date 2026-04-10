# MarketSignal

## Goal
An appliction that calculates market indicators of financial instruments and stores them in a database. This kind of software is useful when making algorithmic stock trading models.

## A bit about the domain
There is a way to globally identify fincancial instruments: by their symbol and market identification code (MIC). An example of such pair is AAPL, XNAS (Nasdaq). However, this system of identification is not universally accepted. Every data provider (such as Alpha Vantage) has their own system of identifiers. Therefore a mapping must be manually maintained.

## How to run
1. `cd` to project's directory
2. Copy `.env.template` file and put it in the same directory. Rename the copy to `.env`.
3. Fill missing values in `.env`. The Alpha Vantage API key can easily be obtained here: `https://www.alphavantage.co/support/#api-key`.
4. `cd docker`
5. `docker compose --env-file=../.env up --build`

## Endpoints
There are 4 endpoints:
1. Downloading raw market data (Open, High, Low, ...) from an external datasource and storing it in the database.
2. Calculating indicators using the downloaded raw data.
3. Fetching calculated indicators.
4. Retrieving job result.

Endpoints can be interacted with via Swagger here: `http://localhost:8080/swagger/index.html`.

### Endpoint 1
Example: 
- symbol: TSCO
- mic: XLON
- dataProvider: ALPHA_VANTAGE

Resulting URL: `http://localhost:8080/api/instruments/raw-data/update?symbol=TSCO&mic=XLON&dataProvider=ALPHA_VANTAGE`

### Endpoint 2
Example: 
- symbol: TSCO
- mic: XLON
- dataProvider: ALPHA_VANTAGE
- indicatorName: SMA
- indicatorArgs: Field=OPEN,Period=15

Resulting URL: `http://localhost:8080/api/instruments/indicators/calculate-values?symbol=TSCO&mic=XLON&dataProvider=ALPHA_VANTAGE&indicatorName=SMA&indicatorArgs=Field%3DOPEN%2CPeriod%3D15`

## Endpoint 3
Example: 
- symbol: TSCO
- mic: XLON
- dataProvider: ALPHA_VANTAGE
- indicatorName: SMA
- indicatorArgs: Field=OPEN,Period=15
- from: 2026-02-14T00:00:00Z
- to: 2026-03-14T00:00:00Z

Resulting URL: `http://localhost:8080/api/instruments/indicators/values?symbol=TSCO&mic=XLON&dataProvider=ALPHA_VANTAGE&indicatorName=SMA&indicatorArgs=Field%3DOPEN%2CPeriod%3D12&from=2026-02-14T00%3A00%3A00Z&to=2026-03-14T00%3A00%3A00Z`

## Endpoint 4
Example: 
- jobId: 89a0d383-b568-4bca-b6eb-e5f4ad68553b

Resulting URL: `http://localhost:8080/api/jobs/89a0d383-b568-4bca-b6eb-e5f4ad68553b/status`