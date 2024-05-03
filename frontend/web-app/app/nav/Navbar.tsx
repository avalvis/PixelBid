import React from 'react'

import Search from './Search';
import Logo from './Logo';

export default function Navbar() {
    return (
        <header style={{ backgroundColor: '#F3CA52' }} className='sticky top-0 z-50 flex justify-between p-5 items-center text-gray-900 shadow-md'>
            <Logo />
            <Search />
            <div>Login</div>
        </header>
    )
}