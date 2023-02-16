# Stage 1: Compile and Build angular codebase

# Use official node image as the base image
FROM node:latest as build

# Set the working directory
WORKDIR /usr/local/app

# Add the source code to app
COPY ./dlock-client/ /usr/local/app/

RUN mv .env.docker .env

# Install all the dependencies
RUN yarn install

# Generate the build of the application
RUN yarn build


# Stage 2: Serve app with nginx server

# Use official nginx image as the base image
FROM nginx:1.23.3

# Copy the build output to replace the default nginx contents.
COPY --from=build /usr/local/app/build /usr/share/nginx/html

COPY ./misc/client/default.conf /etc/nginx/conf.d/default.conf

# Expose port 80, 443
EXPOSE 80
EXPOSE 443