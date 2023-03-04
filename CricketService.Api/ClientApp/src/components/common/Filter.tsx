import React from 'react';
import { useSortBy, useFilters, useGlobalFilter, useAsyncDebounce } from 'react-table';

interface GlobalFilterProps {
    preGlobalFilteredRows: any,
    globalFilter: any,
    setGlobalFilter: any
}

interface NumberRangeColumnFilterProps {
    column: any
}

interface DefaultColumnFilterProps {
    column: any
}

interface SliderColumnFilterProps {
    column: any
}

interface SelectColumnFilterProps {
    column: any
}

export const GlobalFilter: React.FunctionComponent<GlobalFilterProps> = ({
    preGlobalFilteredRows,
    globalFilter,
    setGlobalFilter,
}) => {
    const count = preGlobalFilteredRows.length
    const [value, setValue] = React.useState(globalFilter)
    const onChange = useAsyncDebounce(value => {
        setGlobalFilter(value || undefined)
    }, 200)

    return (
        <span>
            Search:{' '}
            <input
                value={value || ""}
                onChange={e => {
                    setValue(e.target.value);
                    onChange(e.target.value);
                }}
                placeholder={`${count} records...`}
                style={{
                    fontSize: '1.1rem',
                    border: '0',
                }}
            />
        </span>
    )
}

export const DefaultColumnFilter: React.FunctionComponent<DefaultColumnFilterProps> = ({
    column: { filterValue, preFilteredRows, setFilter },
}) => {
    const count = preFilteredRows.length

    return (
        <input
            value={filterValue || ''}
            onChange={e => {
                setFilter(e.target.value || undefined)
            }}
            placeholder={`Search ${count} records...`}
            style={{
               width: '180px'
            }}
        />
    )
}

export const NumberRangeColumnFilter: React.FunctionComponent<NumberRangeColumnFilterProps> = ({
    column: { filterValue = [], preFilteredRows, setFilter, id },
}) => {
    const [min, max] = React.useMemo(() => {
        let min = preFilteredRows.length ? preFilteredRows[0].values[id] : 0
        let max = preFilteredRows.length ? preFilteredRows[0].values[id] : 0
        preFilteredRows.forEach((row: any) => {
            min = Math.min(row.values[id], min)
            max = Math.max(row.values[id], max)
        })
        return [min, max]
    }, [id, preFilteredRows])

    return (
        <div className='number-range-cf'>
            <input
                value={filterValue[0] || ''}
                type="number"
                onChange={e => {
                    const val = e.target.value
                    setFilter((old = []) => [val ? parseInt(val, 10) : undefined, old[1]])
                }}
                placeholder={`Min (${min})`}
            />
            to
            <input
                value={filterValue[1] || ''}
                type="number"
                onChange={e => {
                    const val = e.target.value
                    setFilter((old = []) => [old[0], val ? parseInt(val, 10) : undefined])
                }}
                placeholder={`Max (${max})`}
            />
        </div>
    )
}

export const SelectColumnFilter: React.FunctionComponent<SelectColumnFilterProps> = ({
    column: { filterValue, setFilter, preFilteredRows, id },
}) => {
    const options = React.useMemo(() => {
        const options = new Set()
        preFilteredRows.forEach((row: any) => {
            options.add(row.values[id])
        })
        return [...options.values() as any]
    }, [id, preFilteredRows])

    // Render a multi-select box
    return (
        <select
            value={filterValue}
            onChange={e => {
                setFilter(e.target.value || undefined)
            }}
        >
            <option value="">All</option>
            {options.map((option, i) => (
                <option key={i} value={option}>
                    {option}
                </option>
            ))}
        </select>
    )
}

export const SliderColumnFilter: React.FunctionComponent<SliderColumnFilterProps> = ({
    column: { filterValue, setFilter, preFilteredRows, id },
}) => {
    const [min, max] = React.useMemo(() => {
        let min = preFilteredRows.length ? preFilteredRows[0].values[id] : 0
        let max = preFilteredRows.length ? preFilteredRows[0].values[id] : 0
        preFilteredRows.forEach((row: any) => {
            min = Math.min(row.values[id], min)
            max = Math.max(row.values[id], max)
        })
        return [min, max]
    }, [id, preFilteredRows])

    return (
        <>
            <input
                type="range"
                min={min}
                max={max}
                value={filterValue || min}
                onChange={e => {
                    setFilter(parseInt(e.target.value, 10))
                }}
            />
            <button onClick={() => setFilter(undefined)}>Off</button>
        </>
    )
}