import React from 'react'
import Search from './Search';
import Logo from './Logo';
import LoginButton from './LoginButton';
import { getCurrentUser } from '../actions/authActions';
import UserActions from './UserActions';


export default async function Navbar() {
    const user = await getCurrentUser();
    return (
        <header style={{ backgroundColor: '#F3CA52' }} className='sticky top-0 z-50 flex justify-between p-5 items-center text-gray-900 shadow-md'>
            <Logo />
            <Search />
            {user ? (
                <UserActions user={user} />
            ) : (
                <LoginButton />
            )}
        </header>
    )
}