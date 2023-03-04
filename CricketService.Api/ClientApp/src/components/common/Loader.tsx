import { Grid, TailSpin } from 'react-loader-spinner';

export interface TailSpinLoaderProps {
    
}

export const TailSpinLoader: React.FunctionComponent<TailSpinLoaderProps> = ({ }) => {

    return (
        <div className='tail-spin-loader'>
            <TailSpin
                height="150"
                width="150"
                color="red"
                ariaLabel="tail-spin-loading"
                radius="1"
                wrapperStyle={{}}
                wrapperClass=""
                visible={true}
            />
        </div>
    );
};

export interface GridLoaderProps {
    size?: number,
    color?: string
}

export const GridLoader: React.FunctionComponent<GridLoaderProps> = ({ size = 100 , color = 'blue' }) => {

    return (
        <div className='grid-loader'>
            <Grid
                height={`${size}`}
                width={`${size}`}
                color={color}
                ariaLabel="grid-loading"
                radius="12.5"
                wrapperStyle={{}}
                wrapperClass=""
                visible={true}
            />
        </div>
    );
};

