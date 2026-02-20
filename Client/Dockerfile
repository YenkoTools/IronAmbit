# ─── Build Stage ────────────────────────────────────────────────────────────
FROM node:22-alpine AS build
WORKDIR /app

# Install dependencies (cache-friendly layer)
COPY package*.json ./
RUN npm ci

# Copy the rest of the source
COPY . .

# PUBLIC_ env vars are embedded at build time by Astro
ARG PUBLIC_API_URL=http://localhost:5025/api
ARG PUBLIC_USE_MOCK_API=false
ENV PUBLIC_API_URL=$PUBLIC_API_URL
ENV PUBLIC_USE_MOCK_API=$PUBLIC_USE_MOCK_API

RUN npm run build

# ─── Runtime Stage ──────────────────────────────────────────────────────────
FROM node:22-alpine AS runtime
WORKDIR /app

# Copy only what is needed to serve the static build
COPY --from=build /app/dist            ./dist
COPY --from=build /app/node_modules    ./node_modules
COPY --from=build /app/package.json    ./package.json

EXPOSE 4321

CMD ["node_modules/.bin/astro", "preview", "--host", "0.0.0.0", "--port", "4321"]
