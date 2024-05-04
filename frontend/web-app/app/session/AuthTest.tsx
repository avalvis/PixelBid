'use client'
/**
 * AuthTest Component
 * 
 * This component is used to test the authentication functionality of the application.
 * It provides a button that, when clicked, triggers an update auction test action.
 * The result of this action is then displayed on the screen.
 * 
 * The component uses local state to manage the loading status and the result of the action.
 * 
 * It's part of the broader authentication and auction management functionality of the application.
 */

// Import necessary dependencies
import React, { useState } from 'react' // React and useState hook
import { updateAuctionTest } from '../actions/auctionActions'; // Action to test auction update
import { Button } from 'flowbite-react'; // Button component from flowbite-react library

// Define the AuthTest component
export default function AuthTest() {
    // Define local state variables for loading status and result of the action
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState<any>();

    // Define the function to perform the update auction test action
    function doUpdate() {
        // Reset the result and set loading to true
        setResult(undefined);
        setLoading(true);

        // Perform the update auction test action
        updateAuctionTest()
            .then(res => setResult(res)) // On success, set the result
            .finally(() => setLoading(false)) // Finally, set loading to false
    }

    // Render the component
    return (
        <div className='flex items-center gap-4'>
            {/* Button to trigger the update auction test action */}
            <Button outline isProcessing={loading} onClick={doUpdate}>
                Press to test auth
            </Button>
            {/* Display the result of the action */}
            <div>
                {JSON.stringify(result, null, 2)}
            </div>
        </div>
    )
}