import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { movies: [], loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderMoviesTable(movies) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Year</th>
                        <th>Price</th>
                        <th>Cheap site</th>
                        <th>Poster</th>
                    </tr>
                </thead>
                <tbody>
                    {movies.map(movie =>
                        <tr key={movie.id}>
                            <td>{movie.title}</td>
                            <td>{movie.year}</td>
                            <td>{movie.price}</td>
                            <td>{movie.siteName}</td>
                            <td> <img
                                src={movie.poster}
                                alt="new"
                            /></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderMoviesTable(this.state.movies);

        return (
            <div>
                <h1 id="tabelLabel" >Movie</h1>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('api/MyMovie/GetMovies');
        const data = await response.json();
        debugger;
        this.setState({ movies: data, loading: false });
    }
}
