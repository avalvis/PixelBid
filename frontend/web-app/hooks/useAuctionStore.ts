/**
 * This file defines a Zustand store for managing auction data.
 * Zustand is a small, fast and scaleable bearbones state-management solution.
 * The store has an initial state with empty auctions and zero counts.
 * It provides two actions: setData and setCurrentPrice.
 * setData is used to set the entire state with a new page of auction data.
 * setCurrentPrice is used to update the current high bid of a specific auction.
 */

import { Auction, PagedResult } from "@/types" // Importing necessary types
import { create } from "zustand" // Importing Zustand create function

// Defining the shape of the state
type State = {
    auctions: Auction[]
    totalCount: number
    pageCount: number
}

// Defining the actions that can be performed on the state
type Actions = {
    setData: (data: PagedResult<Auction>) => void
    setCurrentPrice: (auctionId: string, amount: number) => void
}

// Defining the initial state
const initialState: State = {
    auctions: [],
    pageCount: 0,
    totalCount: 0
}

// Creating the Zustand store
export const useAuctionStore = create<State & Actions>((set) => ({
    ...initialState, // Spread the initial state

    // Action to set the state with a new page of auction data
    setData: (data: PagedResult<Auction>) => {
        set(() => ({
            auctions: data.results,
            totalCount: data.totalCount,
            pageCount: data.pageCount
        }))
    },

    // Action to update the current high bid of a specific auction
    setCurrentPrice: (auctionId: string, amount: number) => {
        set((state) => ({
            auctions: state.auctions.map((auction) => auction.id === auctionId
                ? { ...auction, currentHighBid: amount } : auction)
        }))
    }
}))