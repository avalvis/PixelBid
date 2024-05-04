// Import the create function from zustand
import { create } from "zustand"

// Define the state type
type State = {
    pageNumber: number
    pageSize: number
    pageCount: number
    searchTerm: string
    searchValue: string
    orderBy: string
    filterBy: string
    seller?: string
    winner?: string
}

// Define the actions type
type Actions = {
    setParams: (params: Partial<State>) => void
    reset: () => void
    setSearchValue: (value: string) => void
}

// Define the initial state
const initialState: State = {
    pageNumber: 1,
    pageSize: 12,
    pageCount: 1,
    searchTerm: '',
    searchValue: '',
    orderBy: 'title',
    filterBy: 'live',
    seller: undefined,
    winner: undefined

}

// Create a store using zustand. The store will have the state and actions defined above.
export const useParamsStore = create<State & Actions>()((set) => ({
    // Spread the initial state into the store's state
    ...initialState,

    // Define the setParams action. This action takes a Partial<State> as a parameter,
    // and merges it into the current state. If the new params include a pageNumber,
    // it is used, otherwise the pageNumber is reset to 1.
    setParams: (newParams: Partial<State>) => {
        set((state) => {
            if (newParams.pageNumber) {
                return { ...state, pageNumber: newParams.pageNumber }
            }
            else {
                return { ...state, ...newParams, pageNumber: 1 }
            }
        })
    },

    // Define the reset action. This action resets the state to the initial state.
    reset: () => set(initialState),

    setSearchValue: (value: string) =>
        set({ searchValue: value })
}))