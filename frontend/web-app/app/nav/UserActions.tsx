'use client'

import { useParamsStore } from '@/hooks/useParamsStore'
import { Button, Dropdown } from 'flowbite-react'
import { User } from 'next-auth'
import { signOut } from 'next-auth/react'
import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
import React from 'react'
import { AiFillTrophy, AiOutlineLogout } from 'react-icons/ai'
import { GrGamepad } from 'react-icons/gr'
import { HiCog, HiUser } from 'react-icons/hi2'

type Props = {
    user: User

}

export default function UserActions({ user }: Props) {
    const router = useRouter();
    const pathname = usePathname();
    const setParams = useParamsStore(state => state.setParams);
    const [key, setKey] = React.useState(Math.random());

    function setWinner() {
        setParams({ winner: user.username, seller: undefined })
        if (pathname !== '/') router.push('/')
        setKey(Math.random()); // Force re-render
    }

    function setSeller() {
        setParams({ seller: user.username, winner: undefined })
        if (pathname !== '/') router.push('/')
        setKey(Math.random()); // Force re-render
    }

    return (
        <Dropdown key={key} label={`Welcome, ${user.name}`} inline>
            <Dropdown.Item icon={HiUser} onClick={setSeller}>
                My Auctions
            </Dropdown.Item>
            <Dropdown.Item icon={AiFillTrophy} onClick={setWinner}>
                Auctions Won
            </Dropdown.Item>
            <Dropdown.Item icon={GrGamepad} onClick={() => setKey(Math.random())}>
                <Link href="/auctions/create">Sell A Game</Link>
            </Dropdown.Item>
            <Dropdown.Item icon={HiCog} onClick={() => setKey(Math.random())}>
                <Link href="/session">Session (dev only)</Link>
            </Dropdown.Item>
            <Dropdown.Divider />
            <Dropdown.Item icon={AiOutlineLogout} onClick={() => { signOut({ callbackUrl: '/' }); setKey(Math.random()); }}>
                Sign Out
            </Dropdown.Item>
            <Dropdown.Divider />
        </Dropdown>
    )
}
