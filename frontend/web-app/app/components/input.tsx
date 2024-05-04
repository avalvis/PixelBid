/* This code defines a reusable Input component that integrates with react-hook-form for form state management. 
It uses the useController hook from react-hook-form to control the input and manage its state. 
The Input component also accepts a label, type, and showLabel prop for customization. */

import { Label, TextInput } from 'flowbite-react'
import React from 'react'
import { UseControllerProps, useController } from 'react-hook-form'

// Define the props for the Input component
type Props = {
    label: string // The label for the input
    type?: string // The type of the input (e.g., 'text', 'password', etc.)
    showLabel?: boolean // Whether to show the label or not
} & UseControllerProps // Extend the props with UseControllerProps from react-hook-form

// Define the Input component
export default function Input(props: Props) {
    // Use the useController hook from react-hook-form to control the form input
    // This hook returns an object with fieldState and field
    // fieldState contains information about the field's state
    // field contains properties necessary for binding the input to the form
    const { fieldState, field } = useController({ ...props, defaultValue: '' })

    // Render the component
    return (
        <div className='mb-3'> {/* Container div */}
            {props.showLabel && ( // If showLabel prop is true, render the label
                <div className='mb-2 block'>
                    <Label htmlFor={field.name} value={props.label} />
                </div>
            )}
            <TextInput
                {...props} // Spread in the props
                {...field} // Spread in the field properties
                type={props.type || 'text'} // Set the type of the input, default to 'text'
                placeholder={props.label} // Set the placeholder to the label
                // Set the color based on the field state
                // If there's an error, set it to 'failure'
                // If the field is not dirty (i.e., user hasn't interacted with it), set it to ''
                // Otherwise, set it to 'success'
                color={fieldState.error ? 'failure' : !fieldState.isDirty ? '' : 'success'}
                helperText={fieldState.error?.message} // If there's an error, show the error message
            />
        </div>
    )
}