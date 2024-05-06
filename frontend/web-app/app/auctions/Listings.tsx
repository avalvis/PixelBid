'use client'

import React, { useEffect, useState } from 'react';
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { useParamsStore } from '@/hooks/useParamsStore';
import qs from 'query-string';
import EmptyFilter from '../components/EmptyFilter';
import LoadingComponent from '../components/Loading';
import { useAuctionStore } from '@/hooks/useAuctionStore';
import { shallow } from 'zustand/shallow';

export default function Listings() {
    const [loading, setLoading] = useState(true);

    // Get parameters and data from Zustand stores
    const params = useParamsStore(
        state => ({
            pageNumber: state.pageNumber,
            pageSize: state.pageSize,
            searchTerm: state.searchTerm,
            orderBy: state.orderBy,
            filterBy: state.filterBy,
            seller: state.seller,
            winner: state.winner,
        }),
        shallow
    );

    const data = useAuctionStore(
        state => ({
            auctions: state.auctions,
            totalCount: state.totalCount,
            pageCount: state.pageCount,
        }),
        shallow
    );

    // Functions to update store data
    const setData = useAuctionStore(state => state.setData);
    const setParams = useParamsStore(state => state.setParams);

    // Create URL with current parameters
    const url = qs.stringifyUrl({ url: '', query: params });

    // Update page number function
    function setPageNumber(pageNumber: number) {
        setParams({ pageNumber });
    }

    // Fetch data whenever URL or setData changes
    useEffect(() => {
        setLoading(true); // Make sure the loading state is visible
        getData(url).then(data => {
            setData(data);
            setLoading(false);
        });
    }, [url, setData]); // Include setData in the dependency array

    // Show loading component until data is fetched
    if (loading) return <LoadingComponent />;

    return (
        <>
            <Filters />
            {data.totalCount === 0 ? (
                <EmptyFilter showReset />
            ) : (
                <>
                    {data.totalCount > 0 && (
                        <div className="text-neutral-500 mt-4">
                            Showing {data.auctions.length} of {data.totalCount} results
                        </div>
                    )}
                    <div className="grid grid-cols-4 gap-6 mt-8">
                        {data.auctions.map(auction => (
                            <AuctionCard auction={auction} key={auction.id} />
                        ))}
                    </div>
                    <div className="flex justify-center mt-4">
                        <AppPagination
                            currentPage={params.pageNumber}
                            pageCount={data.pageCount}
                            pageChanged={setPageNumber}
                        />
                    </div>
                </>
            )}
        </>
    );
}
