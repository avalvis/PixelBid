import Heading from '@/app/components/Heading'
import React from 'react'
import AuctionForm from '../AuctionForm'

export default function Create() {
    return (
        <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
            <Heading
                title='Do you want to sell a game?'
                subtitle='Enter the details below to create an auction.'
                className='text-center text-[#0A6847] font-serif'
            />
            <AuctionForm />
        </div>
    )
}