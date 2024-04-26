import {Button, Link} from "@mui/material";

export default function NotFound() {
    return (
        <div className="w-full flex flex-col items-center justify-center gap-2">
            <h2>Resources not found!</h2>
            <Link href="/">
                <Button variant="contained">Return Home</Button>
            </Link>
        </div>
    );
}
