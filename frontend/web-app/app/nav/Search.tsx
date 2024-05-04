'use client'

import { useParamsStore } from '@/hooks/useParamsStore'
import { usePathname, useRouter } from 'next/navigation'
import React, { useState } from 'react'
import { FaSearch } from 'react-icons/fa'

export default function Search() {
    const router = useRouter()
    const pathName = usePathname()
    const setParams = useParamsStore(state => state.setParams)
    const setSearchValue = useParamsStore(state => state.setSearchValue)
    const searchValue = useParamsStore(state => state.searchValue)

    function onChange(event: any) {
        setSearchValue(event.target.value)
    }

    function search() {
        if (pathName !== '/') router.push('/')
        setParams({ searchTerm: searchValue })
    }

    return (
        <div style={{ background: '#F6E9B2' }} className='flex w-[50%] items-center border-2 rounded-full shadow-sm'>
            <input
                onKeyDown={(e: any) => {
                    if (e.key === 'Enter') {
                        search()
                    }
                }}
                value={searchValue}
                onChange={onChange}
                type="text"
                placeholder="Search"
                style={{ background: '#F6E9B2' }}
                className='
                    flex-grow
                    pl-5
                    bg-transparent
                    focus:outline-none
                    border-transparent
                    focus:border-transparent
                    focus:ring-0
                    text-sm
                    text-gray-800
                    w-full
                    rounded-full
                    h-full
                '
            />
            <button onClick={search}>
                <FaSearch size={34} className='bg-green-600 text-white rounded-full p-2 cursor-pointer mx-2' />
            </button>
        </div>
    )
}