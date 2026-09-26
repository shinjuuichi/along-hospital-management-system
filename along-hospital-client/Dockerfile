##### Dockerfile #####
## build stage ##
FROM node:22-alpine AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

## run stage ##
FROM nginx:alpine
COPY --from=build /app/dist /run
COPY nginx.conf /etc/nginx/nginx.conf
COPY scripts/env-config.sh /docker-entrypoint.d/40-env-config.sh
RUN chmod +x /docker-entrypoint.d/40-env-config.sh
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]