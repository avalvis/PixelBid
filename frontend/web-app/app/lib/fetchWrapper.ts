/**
 * This module provides a wrapper around the fetch API for making HTTP requests.
 * It includes methods for GET, POST, PUT, and DELETE requests.
 * Each method automatically includes the appropriate headers, including an authorization token if one is available.
 * The base URL for requests is 'http://localhost:6001/'.
 */

import { getTokenWorkaround } from "@/app/actions/authActions";

// Base URL for all requests read from env.local
const baseUrl = process.env.API_URL;

// Function to make a GET request
async function get(url: string) {
    const requestOptions = {
        method: 'GET',
        // Get the headers for the request
        headers: await getHeaders()
    }

    // Make the request and handle the response
    const response = await fetch(baseUrl + url, requestOptions);
    return await handleResponse(response);
}

// Function to make a POST request
async function post(url: string, body: {}) {
    const requestOptions = {
        method: 'POST',
        // Get the headers for the request
        headers: await getHeaders(),
        // Convert the body to a JSON string
        body: JSON.stringify(body)
    }
    // Make the request and handle the response
    const response = await fetch(baseUrl + url, requestOptions);
    return await handleResponse(response);
}

// Function to make a PUT request
async function put(url: string, body: {}) {
    const requestOptions = {
        method: 'PUT',
        // Get the headers for the request
        headers: await getHeaders(),
        // Convert the body to a JSON string
        body: JSON.stringify(body)
    }
    // Make the request and handle the response
    const response = await fetch(baseUrl + url, requestOptions);
    return await handleResponse(response);
}

// Function to make a DELETE request
async function del(url: string) {
    const requestOptions = {
        method: 'DELETE',
        // Get the headers for the request
        headers: await getHeaders()
    }
    // Make the request and handle the response
    const response = await fetch(baseUrl + url, requestOptions);
    return await handleResponse(response);
}

// Function to get the headers for a request
async function getHeaders() {
    // Get the token
    const token = await getTokenWorkaround();
    // Set the content type header
    const headers = { 'Content-type': 'application/json' } as any;
    // If a token is available, add it to the headers
    if (token) {
        headers.Authorization = 'Bearer ' + token.access_token
    }
    return headers;
}

// Function to handle the response from a request
async function handleResponse(response: Response) {
    // Get the response text
    const text = await response.text();
    let data;
    try {
        // Try to parse the response text as JSON
        data = JSON.parse(text);
    } catch (error) {
        // If parsing as JSON fails, use the raw response text
        data = text;
    }

    // If the response was successful, return the data (or the status text if no data is available)
    if (response.ok) {
        return data || response.statusText;
    } else {
        // If the response was not successful, return an error object
        const error = {
            status: response.status,
            message: typeof data === 'string' && data.length > 0 ? data : response.statusText
        }
        return { error };
    }
}

// Export the fetch wrapper functions
export const fetchWrapper = {
    get,
    post,
    put,
    del
}