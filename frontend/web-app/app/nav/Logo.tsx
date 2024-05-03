'use client'

import { useParamsStore } from '@/hooks/useParamsStore';
import React from 'react'
import { SlGameController } from "react-icons/sl";

export default function Logo() {

    const reset = useParamsStore(state => state.reset)
    return (

        <div onClick={reset} style={{ color: '#0A6847' }} className='cursor-pointer flex items-center gap-3 text-3xl font-semibold'>
            <SlGameController size={40} />
            <div className="jersey-15-regular">PixelBid</div>
        </div>

    )
}
