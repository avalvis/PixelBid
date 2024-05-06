'use client'

/**
 * BidList Component
 * 
 * This component is responsible for displaying a list of bids for a specific auction.
 * It fetches the bids from the server, displays the current high bid, and allows the user to place a new bid if they are eligible.
 * 
 * Props:
 * - user: The currently logged-in user
 * - auction: The auction for which the bids are being displayed
 */

// Import necessary dependencies
import { getBidsForAuction } from '@/app/actions/auctionActions'
import Heading from '@/app/components/Heading'
import { useBidStore } from '@/hooks/useBidStore'
import { Auction, Bid } from '@/types'
import { User } from 'next-auth'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-hot-toast'
import BidItem from './BidItem'
import BidForm from './BidForm'
import { numberWithCommas } from '@/app/lib/numberWithComma'
import EmptyFilter from '@/app/components/EmptyFilter'

type Props = {
    user: User | null
    auction: Auction
}

export default function BidList({ user, auction }: Props) {
    // Define state variables
    const [loading, setLoading] = useState(true);
    const bids = useBidStore(state => state.bids);
    const setBids = useBidStore(state => state.setBids);
    const open = useBidStore(state => state.open);
    const setOpen = useBidStore(state => state.setOpen);
    const openForBids = new Date(auction.auctionEnd) > new Date();

    // Calculate the highest bid
    const highBid = bids.reduce((prev, current) => prev > current.amount
        ? prev
        : current.bidStatus.includes('Accepted')
            ? current.amount
            : prev, 0)

    // Fetch bids when the component mounts
    useEffect(() => {
        getBidsForAuction(auction.id)
            .then((res: any) => {
                if (res.error) {
                    throw res.error
                }
                setBids(res as Bid[]);
            }).catch(err => {
                toast.error(err.message);
            }).finally(() => setLoading(false))
    }, [auction.id, setLoading, setBids])

    // Update the 'open' state variable when 'openForBids' changes
    useEffect(() => {
        setOpen(openForBids);
    }, [openForBids, setOpen]);

    // Display a loading message while the bids are being fetched
    if (loading) return <span>Loading bids...</span>

    // Render the list of bids
    return (
        <div className='rounded-lg shadow-md'>
            <div className='py-2 px-4 '>
                <div className='sticky top-0 p-2'>
                    <Heading
                        title={`The top bid right now is: $${numberWithCommas(highBid)}`}
                        className="font-custom bg-custom-yellow shadow-custom rounded-custom p-custom hover:scale-105 transition-transform duration-300 ease-in-out"
                    />
                </div>
            </div>

            <div className='overflow-auto h-[400px] flex flex-col-reverse px-2'>
                {bids.length === 0 ? (
                    <EmptyFilter title='No bids for this item'
                        subtitle='Be the first to make one' />
                ) : (
                    <div>
                        {bids.map(bid => (
                            <BidItem key={bid.id} bid={bid} />
                        ))}
                    </div>
                )}
            </div>

            <div className='px-2 pb-2 text-gray-500'>
                {!open ? (
                    // Display a message if the auction is closed
                    <div className='flex items-center justify-center p-2 text-lg font-semibold'>
                        Sorry, this auction has finished
                    </div>
                    // Add a condition to check if the user is logged in
                ) : !user ? (
                    <div className='flex items-center justify-center p-2 text-lg font-semibold'>
                        You need to login first to make a bid
                    </div>
                    // Add a condition to check if the user is the seller
                ) : user && user.username === auction.seller ? (
                    <div className='flex items-center justify-center p-2 text-lg font-semibold'>
                        You cannot bid on your own auction
                    </div>
                ) : (
                    <BidForm auctionId={auction.id} highBid={highBid} />
                )}
            </div>
        </div>
    )
}