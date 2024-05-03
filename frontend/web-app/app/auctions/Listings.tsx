'use client'

/* This is a React component that fetches and displays the list of auctions. 
It uses the useState hook to create a state variable for storing the fetched data (data), 
and the useParamsStore hook to get the current parameters (params) and the setParams function from the Zustand store. 

The URL for fetching the data is created with the current parameters as query parameters. 
This URL is updated whenever the parameters in the Zustand store change. 

The data is fetched in a useEffect hook, which runs whenever the URL changes. 
The fetched data is then stored in the state variable (data).

If the data is not yet fetched, a loading message is displayed. 

Once the data is fetched, it is rendered as a list of AuctionCard components. 
Each AuctionCard component receives an auction object as a prop. 

The component also renders a Filters component for filtering the auctions, 
and an AppPagination component for pagination. 
The AppPagination component receives the current page number, the total page count, 
and a function for setting the page number (setPageNumber) as props. */


import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import { Auction, PagedResult } from '@/types';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import qs from 'query-string';
import EmptyFilter from '../components/EmptyFilter';
import LoadingComponent from '../components/Loading';

// Define the Listings component
export default function Listings() {
    // Create a state variable for storing the fetched data
    const [data, setData] = useState<PagedResult<Auction>>();

    // Get the current parameters from the Zustand store
    const params = useParamsStore(state => ({
        pageNumber: state.pageNumber,
        pageSize: state.pageSize,
        searchTerm: state.searchTerm,
        orderBy: state.orderBy,
        filterBy: state.filterBy
    }), shallow)

    // Get the setParams function from the Zustand store
    const setParams = useParamsStore(state => state.setParams)

    // Create a URL with the current parameters as query parameters
    const url = qs.stringifyUrl({ url: '', query: params })

    // Define a function for setting the page number
    function setPageNumber(pageNumber: number) {
        setParams({ pageNumber })
    }

    // Fetch data whenever the URL changes
    useEffect(() => {
        getData(url).then(data => {
            setData(data);
        })
    }, [url])

    // If data is not yet fetched, render a loading message
    if (!data) return <LoadingComponent />

    // Render the fetched data
    return (
        <>
            <Filters />

            {data.totalCount === 0 ? (
                <EmptyFilter showReset />
            ) : (

                <>
                    {data.totalCount > 0 && <div className='text-neutral-500 mt-4'>Showing {data.results.length} of {data.totalCount} results</div>}
                    <div className='grid grid-cols-4 gap-6 mt-8'> {/* Replace 8 with the amount of space you want to add */}
                        {data.results.map(auction => (
                            <AuctionCard auction={auction} key={auction.id} />
                        ))}
                    </div>
                    <div className='flex justify-center mt-4'>
                        <AppPagination currentPage={params.pageNumber} pageCount={data.pageCount} pageChanged={setPageNumber} />
                    </div>
                </>

            )}

        </>
    )
}