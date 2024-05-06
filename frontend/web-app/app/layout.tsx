import { getCurrentUser } from './actions/authActions'
import './globals.css'
import Navbar from './nav/Navbar'
import SignalRProvider from './providers/SignalRProvider'
import ToasterProvider from './providers/ToasterProvider'

export const metadata = {
  title: 'Pixelbid',
  description: 'Developed by Antonis Valvis',
}

export default async function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  const user = await getCurrentUser();
  return (
    <html lang="en">
      <body style={{ backgroundColor: '#F6E9B2' }}>
        <ToasterProvider />
        <Navbar />
        <main style={{ backgroundColor: '#F6E9B2' }} className="container mx-auto px-5 pt-10">
          <SignalRProvider user={user}>
            {children}
          </SignalRProvider>

        </main>

      </body>
    </html>
  )
}
