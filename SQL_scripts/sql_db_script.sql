CREATE TABLE flight_records (
    icao24 TEXT,          
    callsign TEXT,       
    origincountry TEXT,          
    timeposition BIGINT,
    lastcontact BIGINT,
    longitude DOUBLE PRECISION,
    latitude DOUBLE PRECISION,
    baroaltitude DOUBLE PRECISION,
    onground BOOLEAN,
    velocity DOUBLE PRECISION
);