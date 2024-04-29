import Client from "ioredis";
import Redlock from "redlock";

const redis = new Client({
  host: process.env.NEXT_SERVER_REDIS_URL,
  port: process.env.NEXT_SERVER_REDIS_PORT,
  password: process.env.NEXT_SERVER_REDIS_PASSWORD,
});

const redLock = new Redlock([redis], {
  driftFactor: 0.01,
  retryCount: 10,
  retryDelay: 200,
  retryJitter: 200,
  automaticExtensionThreshold: 500,
});

export { redis, redLock };
