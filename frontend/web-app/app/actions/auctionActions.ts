/*
 * This module provides functions for performing CRUD operations on auctions.
 * It uses the fetchWrapper module to make HTTP requests, and the next/cache module to revalidate paths.
 */

'use server'

import { Auction, Bid, PagedResult } from "@/types"

import { FieldValues } from "react-hook-form"
import { revalidatePath } from "next/cache"
import { fetchWrapper } from "../lib/fetchWrapper"

// Fetches a page of auctions based on the provided query.
export async function getData(query: string): Promise<PagedResult<Auction>> {
    return await fetchWrapper.get(`search${query}`)
}

// Updates a specific auction with randomly generated play hours for testing purposes.
export async function updateAuctionTest() {
    const data = {
        playHours: Math.floor(Math.random() * 100) + 1
    }

    return await fetchWrapper.put(`auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c`, data)
}

// Fetches detailed data for a specific auction.
export async function getDetailedViewData(id: string): Promise<Auction> {
    return await fetchWrapper.get(`auctions/${id}`);
}

// Creates a new auction with the provided data.
export async function createAuction(data: FieldValues) {
    return await fetchWrapper.post('auctions', data)
}

// Updates a specific auction with the provided data, then revalidates the path.
export async function updateAuction(data: FieldValues, id: string) {
    const res = await fetchWrapper.put(`auctions/${id}`, data);
    revalidatePath(`/auctions/${id}`);
    return res;
}

// Deletes a specific auction and then revalidates the path.
export async function deleteAuction(id: string) {
    const res = await fetchWrapper.del(`auctions/${id}`);
    revalidatePath(`/auctions/${id}`);
    return res;
}

// Fetches all bids for a specific auction.
export async function getBidsForAuction(id: string): Promise<Bid[]> {
    return await fetchWrapper.get(`bids/${id}`);
}


// Places a bid for a specific auction with the provided amount.
export async function placeBidForAuction(auctionId: string, amount: number) {
    return await fetchWrapper.post(`bids?auctionId=${auctionId}&amount=${amount}`, {})
}