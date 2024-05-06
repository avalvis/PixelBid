/**
 * This file defines a Zustand store for managing bid data.
 * The store has an initial state with empty bids and open status set to true.
 * It provides three actions: setBids, addBid, and setOpen.
 * setBids is used to replace the current bids with a new set of bids.
 * addBid is used to add a new bid to the current bids, if it doesn't already exist.
 * setOpen is used to update the open status.
 */

import { Bid } from "@/types" // Importing necessary types
import { create } from "zustand" // Importing Zustand create function

// Defining the shape of the state
type State = {
    bids: Bid[]
    open: boolean
}

// Defining the actions that can be performed on the state
type Actions = {
    setBids: (bids: Bid[]) => void
    addBid: (bid: Bid) => void
    setOpen: (value: boolean) => void
}

// Creating the Zustand store
export const useBidStore = create<State & Actions>((set) => ({
    bids: [], // Initial state for bids
    open: true, // Initial state for open status

    // Action to replace the current bids with a new set of bids
    setBids: (bids: Bid[]) => {
        set(() => ({
            bids
        }))
    },

    // Action to add a new bid to the current bids, if it doesn't already exist
    addBid: (bid: Bid) => {
        set((state) => ({
            bids: !state.bids.find(x => x.id === bid.id) ? [bid, ...state.bids] : [...state.bids]
        }))
    },

    // Action to update the open status
    setOpen: (value: boolean) => {
        set(() => ({
            open: value
        }))
    }
}))