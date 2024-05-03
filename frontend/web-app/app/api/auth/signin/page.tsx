import EmptyFilter from '@/app/components/EmptyFilter'
import React from 'react'

export default function Page({ searchParams }: { searchParams: { callbackUrl: string } }) {
  return (
    <EmptyFilter
      title='You are not logged in'
      subtitle='click below to sign in'
      showLogin
      callbackUrl={searchParams.callbackUrl}
    />
  )
}
