import {Inter} from "next/font/google";
import "./globals.css";
import RootLayoutWrapper from "@/wrapper/client/RootLayoutWrapper";
import {getServerSession} from "next-auth";

const inter = Inter({subsets: ["latin"]});

export const metadata = {
    title: "QWiz",
    description: "Question Bank Management",
};

export default async function RootLayout({children}) {
    const session = await getServerSession();
    return (
        <html lang="en">
        <body className={inter.className}>
        <RootLayoutWrapper {...{session}}>
            {children}
        </RootLayoutWrapper>
        </body>
        </html>
    );
}
